using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestatServer.Models;

namespace TestatServer.Services
{
    public class DataService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AccountService _accountService;
        private readonly Random _random;

        public DataService(UserManager<ApplicationUser> userManager, AccountService accountService  ) {
            _userManager = userManager;
            _accountService = accountService;
            _random = new Random();
        }

        public ApplicationUser CreateUser(string userName, string firstName, string lastName)
        {
            var user = new ApplicationUser() { UserName = userName, FirstName = firstName, LastName = lastName, Email = userName + "@test.ch" };
            _userManager.CreateAsync(user, "1234").Wait();
            user.Account = _accountService.AddAccount(user);
            return user;
        }
        public void EnsureData()
        {
            if (!_userManager.Users.Any())
            {
                CreateUser("user1", "Bob", "Müller");
                CreateUser("user2", "Michael", "Gfeller");
                CreateUser("user3", "Lisa", "Meier");

                var names = _userManager.Users.ToList().Select(x=>_accountService.GetAccount(x).AccountNr).ToList();
                CreateTransactions(names, 500, new DateTime(2016, 10, 5));
            }
        }

        public void CreateTransactions(List<string> names, int count, DateTime next)
        {
            try
            {
                _accountService.AddTransaction(names[_random.Next(0, 3)], names[_random.Next(0, 3)], _random.Next(0, 150), next);
            }
            catch (Exception )
            {
                //nothing
            }
            if (count > 0)
            {
                CreateTransactions(names, count - 1, next + TimeSpan.FromHours(3));
            }
        }
    }
}
