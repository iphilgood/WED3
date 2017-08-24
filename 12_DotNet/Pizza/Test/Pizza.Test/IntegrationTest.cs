using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Pizza.Controllers.Api;
using Pizza.Data;
using Pizza.Models;
using Pizza.Models.OrderViewModels;
using Pizza.Services;
using Xunit;

namespace Pizza.Test
{
    public class ArchiveControllerTest
    {
        private readonly SUT _sut;

        public ArchiveControllerTest()
        {
            _sut = new SUT();
        }

        [Fact]
        public async Task<Task> CreateOrder()
        {
            var wrongOrder = JsonConvert.SerializeObject(new NewOrderViewModel() { Name = "X" });
            var okOrder = JsonConvert.SerializeObject(new NewOrderViewModel() { Name = "Hawaii" });

            var result = await _sut.Request("/api/orders", null, "POST", new StringContent(wrongOrder, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            var result2 = await _sut.Request("/api/orders", _sut.Admin, "POST", new StringContent(wrongOrder, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, result2.StatusCode);

            var result3 = await _sut.Request("/api/orders", _sut.Admin, "POST", new StringContent(okOrder, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, result3.StatusCode);

            Assert.Equal(1, _sut.Server.Host.Services.GetService<ApplicationDbContext>().Orders.Count());

            return Task.CompletedTask;
        }      
    }
   

    public class SUT //System under test 
    {
        internal TestServer Server;
        internal HttpClient Client;
      
        public SUT()
        {
            var contentRootPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, @"..\..\..\..\..\src\Pizza");

            var webHost = new WebHostBuilder()
                .UseContentRoot(contentRootPath)
                .UseWebRoot(Path.Combine(contentRootPath, "wwwroot"))
                .UseEnvironment("TEST")
                .UseStartup<TestStartup>();


            Server = new TestServer(webHost);          
            Client = Server.CreateClient();
            Server.Host.Services.GetService<UserManager<ApplicationUser>>().CreateAsync(new ApplicationUser() {UserName = "admin@admin.ch"}, "123456").Wait();
            Admin = Server.Host.Services.GetService<SecurityService>().GetToken(new AuthRequest(){Username = "admin@admin.ch", Password = "123456"}).Result.Token;
        }



         
        public async Task<HttpResponseMessage> Request(string url, string token, string method = "POST", HttpContent content = null)
        {
            var result = await Server.CreateRequest(url).And(req => {
                    req.Content = content;
                    if (token != null)
                    {
                        req.Headers.Add("Authorization", $"Bearer {token}");
                    }
                    req.Headers.Add("accept", "application/json");
            })
                .SendAsync(method);
            return result;
        }

   

        public dynamic Admin { get; set; }        
    }


    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TEST"));
        }
    }
}
