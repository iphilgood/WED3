using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestatServer.Models;
using TestatServer.Models.ViewModels;
using TestatServer.Services;

namespace TestatServer.Controllers
{
    [Route("accounts")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        ///<summary>
        /// Get account owner
        ///</summary>
        [HttpGet]
        public AccountOwnerViewModel GetAccountOwner()
        {
            var accountNr = User.FindFirst(SecurityClaims.AccountIdClaim).Value;
            var account = _accountService.GetAccount(accountNr);

            return new AccountOwnerViewModel()
            {
                AccountNr = account.AccountNr,
                Amount = account.Amount,
                OwnerId = account.Owner.Id,
                Owner = new OwnerViewModel()
                {
                    Firstname = account.Owner.FirstName,
                    Lastname = account.Owner.LastName,
                    AccountNr = account.AccountNr,
                    Login = account.Owner.UserName
                }
            };
        }

        ///<summary>
        /// Get information of an account
        ///</summary>
        [HttpGet("{accountNr}")]
        public AccountViewModel GetAccount(string accountNr)
        {
            var account = _accountService.GetAccount(accountNr);

            return new AccountViewModel()
            {
                AccountNr = account.AccountNr,
                Owner = new OwnerViewModel()
                {
                    Firstname = account.Owner.FirstName,
                    Lastname = account.Owner.LastName
                }
            };
        }

        ///<summary>
        /// Transact from authenticated user to specified user
        ///</summary>
        [HttpPost("transactions")]
        public TransactionResultViewModel AddTransaction([FromBody] TransactionViewModel incTransaction)
        {
            var accountNr = User.FindFirst(SecurityClaims.AccountIdClaim).Value;
            var account = _accountService.GetAccount(accountNr);

            var result = _accountService.AddTransaction(accountNr, incTransaction.Target, incTransaction.Amount, new DateTime());

            return new TransactionResultViewModel()
            {
                From = account.AccountNr,
                Target = result.Target,
                Amount = result.Amount,
                Total = account.Amount,
                Date = result.Date
            };
        }

        ///<summary>
        /// Get transactions between given date
        ///</summary>
        [HttpGet("transactions")]
        public TransactionSearchResult GetTransactions([FromQuery] TransactionSearchQuery query)
        {
            var accountNr = User.FindFirst(SecurityClaims.AccountIdClaim).Value;
            return _accountService.GetTransactions(accountNr, query);
        }
    }
}
