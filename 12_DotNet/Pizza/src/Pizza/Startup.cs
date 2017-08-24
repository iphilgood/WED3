using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Pizza.Data;
using Pizza.Exceptions;
using Pizza.Models;
using Pizza.Services;
using Swashbuckle.Swagger.Model;

namespace Pizza
{
    public class Startup
    {
        private RsaSecurityKey _key;
        const string TokenAudience = "self";
        const string TokenIssuer = "pizza";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            ConfigureDatabase(services);

            ConfigureIdentityService(services);
            ConfigureAuthorizationService(services);
            ConfigureTokenAuth(services);

            services.AddOptions();
            services.Configure<FacebookSettings>(Configuration.GetSection("FacebookSettings"));
            services.AddSession();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<OrderService, OrderService>();
            services.AddTransient<DataService, DataService>();
            services.AddTransient<SecurityService, SecurityService>();


            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ConfigureDev(app, env);

            app.UseExceptionHandler(ConfigureExceptionHandler);
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi();

            ConfigureRestMvc(app);
            ConfigureMvc(app);
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestatServer");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Use In-Memory Database instead just like above
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        #region configure service helpers
        private void ConfigureTokenAuth(IServiceCollection services)
        {
            RSA rsa = RSA.Create();
            rsa.KeySize = 2048;
            RSAParameters rsaKeyInfo = rsa.ExportParameters(true);

            _key = new RsaSecurityKey(rsaKeyInfo);

            var tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256Signature)
            };
            services.AddSingleton(tokenOptions);
        }

        private static void ConfigureAuthorizationService(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Founders", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Name, "mgfeller@hsr.ch", "sgehrig@hsr.ch", "mstolze@hsr.ch");
                });

                options.AddPolicy("ElevatedRights", policy =>
                            policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator")
                );
            });
        }

        private static void ConfigureIdentityService(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        #endregion

        #region configure helpers

        private static void ConfigureMvc(IApplicationBuilder app)
        {
            app.UseIdentity();

            var settings = app.ApplicationServices.GetService<IOptions<FacebookSettings>>().Value;
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = settings.AppId,
                AppSecret = settings.AppSecret
            });

            app.UseSession(new SessionOptions() {IdleTimeout = TimeSpan.FromMinutes(30)});
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id:int?}");
                routes.MapRoute(
                    name: "shopname",
                    template: "shopname",
                    defaults: new {controller = "Home", action = "name"});
            });
        }

        private void ConfigureRestMvc(IApplicationBuilder app)
        {
            app.MapWhen(
                context => context.Request.GetTypedHeaders().Accept.Any(x => x.MediaType == "application/json"),
                builder =>
                {
                    builder.UseJwtBearerAuthentication(new JwtBearerOptions
                    {
                        AutomaticAuthenticate = true,
                        AutomaticChallenge = true,
                        TokenValidationParameters = GetTokenValidationParameters()
                    });
                    builder.UseMvc();
                });
        }


        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = TokenIssuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = TokenAudience,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
        }

        private void ConfigureDev(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

                app.ApplicationServices.GetService<DataService>().EnsureData(Configuration["AdminPwd"]);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
        }

        private void ConfigureExceptionHandler(IApplicationBuilder errorApp)
        {
            errorApp.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error as ServiceException;

                var metadata = new
                {
                    Message = "An unexpected error occurred! The error ID will be helpful to debug the problem",
                    DateTime = DateTimeOffset.Now,
                    RequestUri = new Uri(context.Request.Host.ToString() + context.Request.Path.ToString() + context.Request.QueryString, UriKind.RelativeOrAbsolute),
                    ErrorId = exception == null ? "Unkown" : exception.Code,
                    Type = exception == null ? ServiceExceptionType.Unkown : exception.Type,
                    Exception = exception
                };
                context.Response.ContentType = "application/json";
                if (exception != null)
                {
                    context.Response.StatusCode = (int) exception.Type;
                }

                await context.Response.WriteAsync(JsonConvert.SerializeObject(metadata));
            });
        }

        #endregion

    }


    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class FacebookSettings
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
