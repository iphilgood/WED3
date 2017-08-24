using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestatServer.Data;
using TestatServer.Exceptions;
using TestatServer.Models;
using TestatServer.Services;

namespace TestatServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            ConfigureDatabase(services);


            ConfigureIdentityService(services);

            services.AddOptions();
            services.AddSession();


            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<DataService, DataService>();
            services.AddScoped<SecurityService, SecurityService>();
            services.AddSingleton<SecurityUtil, SecurityUtil>();

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
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestatServer");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        }

        #region configure service helpers

        private static void ConfigureIdentityService(IServiceCollection services)
        {
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

        private void ConfigureRestMvc(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthentication(app.ApplicationServices.GetService<SecurityUtil>().JwtBearerOptions);
            app.UseMvc();
        }

        private void ConfigureDev(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Database.Migrate(); // nicht nötig bei inMemory
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                app.ApplicationServices.GetService<DataService>().EnsureData();
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
                    Type = exception?.Type ?? ServiceExceptionType.Unkown,
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
}
