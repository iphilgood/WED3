using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using TestatServer.Models;
using TestatServer.Models.ViewModels;
using TestatServer.Services;
using Xunit;

namespace TestatServer.Test
{
    public class ArchiveControllerTest
    {
        private readonly SUT _sut;

        public ArchiveControllerTest()
        {
            _sut = new SUT();
        }

        [Fact]
        public async void GetOwnAccount()
        {
            HttpResponseMessage result = await _sut.Request("/accounts", _sut.AdminToken, "GET");
            var account = JsonConvert.DeserializeObject<AccountOwnerViewModel>(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(account.AccountNr, _sut.AdminUser.Account.AccountNr);
            Assert.Equal(account.Amount, 1000);
            Assert.Equal(account.Owner.Login, _sut.AdminUser.UserName);
        }

        [Fact]
        public async void GetOtherAccount()
        {
            HttpResponseMessage result = await _sut.Request("/accounts/"+ _sut.TestUser1.Account.AccountNr, _sut.AdminToken, "GET");
            var account = JsonConvert.DeserializeObject<AccountViewModel>(result.Content.ReadAsStringAsync().Result);
            Assert.Equal(account.AccountNr, _sut.TestUser1.Account.AccountNr);
        }

        [Fact]
        public async void AddTranscationAboveLimit()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new {Target = _sut.TestUser1.Account.AccountNr, Amount = 2000}), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _sut.Request("/accounts/transactions", _sut.AdminToken, "POST", content);
            Assert.Equal(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void WrongTarget()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Target = "XXXXXXXXX", Amount = 500 }), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _sut.Request("/accounts/transactions", _sut.AdminToken, "POST", content);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(_sut.AdminUser.Account.Amount, 1000);
            Assert.Equal(_sut.TestUser1.Account.Amount, 1000);
        }

        [Fact]
        public async void CorrectTransaction()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Target = _sut.TestUser1.Account.AccountNr, Amount = 500 }), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _sut.Request("/accounts/transactions", _sut.AdminToken, "POST", content);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            Assert.Equal(500, JsonConvert.DeserializeObject<TransactionResultViewModel>(result.Content.ReadAsStringAsync().Result).Total);
            Assert.Equal(1500, _sut.GetAccount(_sut.TestUser1).Amount);
            Assert.Equal(500, _sut.GetAccount(_sut.AdminUser).Amount);
        }


        [Fact]
        public void CheckPaging()
        {
            var query = new TransactionSearchQuery() { Count = 20 };
            var query1 = new TransactionSearchQuery() { Count = 20, Skip = 1};
            var query2 = new TransactionSearchQuery();

            using (var scope = _sut.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<AccountService>();
                var searchResult = service.GetTransactions(_sut.User1.Account.AccountNr, query);
                var searchResult1 = service.GetTransactions(_sut.User1.Account.AccountNr, query1);
                var searchResult2 = service.GetTransactions(_sut.User1.Account.AccountNr, query2);
                Assert.Equal(20, searchResult.Result.Length);
                Assert.Equal(20, searchResult1.Result.Length);
                Assert.True(searchResult1.Query.Resultcount > 20);

                Assert.Equal(0, searchResult2.Result.Length);


                Assert.True(searchResult1.Result[0].Id == searchResult.Result[1].Id);
            }
        }
    }

    public class SUT //System under test
    {
        internal TestServer Server;
        internal HttpClient Client;

        public ApplicationUser User1 { get; set; }
        internal ApplicationUser AdminUser { get; set; }
        internal ApplicationUser TestUser1 { get; set; }
        public string AdminToken { get; set; }
        public string User1Token { get; set; }

        public SUT()
        {
            var contentRootPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                @"../../../../../TestatServer");

            var webHost = new WebHostBuilder()
                .UseContentRoot(contentRootPath)
                .UseEnvironment("TEST")
                .UseStartup<TestStartup>();

            Server = new TestServer(webHost);
            Client = Server.CreateClient();

            Server.Host.Services.GetService<DataService>().EnsureData();
            Server.Host.Services.GetService<DataService>().CreateUser("admin@admin.ch", "F", "L");
            Server.Host.Services.GetService<DataService>().CreateUser("user1@test.ch", "F", "L");

            User1 = GetAccount("user1").Owner;
            TestUser1 = GetAccount("user1@test.ch").Owner;
            AdminUser = GetAccount("admin@admin.ch").Owner;

            AdminToken = Server.Host.Services.GetService<SecurityService>().GetToken(new AuthRequest() {Username = "admin@admin.ch", Password = "1234"}).Result.Token;
            User1Token = Server.Host.Services.GetService<SecurityService>().GetToken(new AuthRequest() {Username = "user1", Password = "1234"}).Result.Token;
        }

        public Account GetAccount(ApplicationUser user)
        {
            using (var scope = Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<AccountService>();
                return service.GetAccount(user);
            }
        }

        public Account GetAccount(string userName)
        {
            using (var scope = Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<AccountService>();
                return service.GetAccountByName(userName);
            }
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
    }
}
