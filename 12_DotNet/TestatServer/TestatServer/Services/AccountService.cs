using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestatServer.Data;
using TestatServer.Exceptions;
using TestatServer.Models;

namespace TestatServer.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;


        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Account GetAccount(string accountNr)
        {
            var account = _context.Accounts.Include(x=>x.Owner).FirstOrDefault(x=>x.AccountNr == accountNr);
            return account;
        }

        public Account GetAccountByName(string name)
        {
            var account = _context.Accounts.Include(x => x.Owner).FirstOrDefault(x => x.Owner.UserName == name);
            return account;
        }

        public Account GetAccount(ApplicationUser onwer)
        {
            var account = _context.Accounts.Include(x => x.Owner).FirstOrDefault(x => x.Owner == onwer);
            return account;
        }

        public Account AddAccount(ApplicationUser owner)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var account = new Account
                {
                    AccountNr = Guid.NewGuid().ToString(),
                    Owner = owner,
                    Amount = 1000
                };
                _context.Transactions.Add(new Transaction { From = "0000000", Target = account.AccountNr, Amount = 1000, Date = DateTime.Now });
                _context.Accounts.Add(account);
                _context.SaveChanges();
                transaction.Commit();
                return account;
            }
        }

        public Transaction AddTransaction(string fromId, string targetId, double amount, DateTime? date)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var from = _context.Accounts.FirstOrDefault(x => x.AccountNr == fromId);
                var target = _context.Accounts.FirstOrDefault(x => x.AccountNr == targetId);
                if (from == null || target == null)
                {
                    throw new ServiceException(ServiceExceptionType.NotFound);
                }
                if (fromId == targetId || amount <= 0 || from.Amount < amount)
                {
                    throw new ServiceException(ServiceExceptionType.ForbiddenByRule);
                }
                var fromTransaction = new Transaction {From = fromId, Target = targetId, Amount = -amount, Date = date.GetValueOrDefault(DateTime.Now)};
                _context.Transactions.Add(fromTransaction);

                _context.Transactions.Add(new Transaction {From = fromId, Target = targetId, Amount = amount, Date = date.GetValueOrDefault(DateTime.Now)});
                from.Amount -= amount;
                target.Amount += amount;

                _context.SaveChanges();
                transaction.Commit();
                return fromTransaction;
            }
        }

        public TransactionSearchResult GetTransactions(string accountId, TransactionSearchQuery query)
        {
            if (!(query.Count.HasValue || (query.FromDate.HasValue && query.ToDate.HasValue)))
            {
                return new TransactionSearchResult() {Query = query};
            }

            var filterQuery = _context.Transactions.Where(x =>
                x.From == accountId || x.Target == accountId
            );

            if (query.FromDate.HasValue && query.ToDate.HasValue)
            {
                filterQuery = filterQuery.Where(x => x.Date > query.FromDate.Value && x.Date < query.ToDate.Value);
            }
            filterQuery = filterQuery.OrderByDescending(x => x.Date);

            if (query.Skip.HasValue)
            {
                filterQuery = filterQuery.Skip(query.Skip.Value);
            }

            if (query.Count.HasValue)
            {
                filterQuery = filterQuery.Take(query.Count.Value);
            }

            query.Resultcount = _context.Transactions.Count(x =>
                x.From == accountId || x.Target == accountId
            );
            return new TransactionSearchResult() {Result = filterQuery.ToArray(), Query = query};
        }
    }

    public class TransactionSearchResult
    {
        public Transaction[] Result { get; set; } = new Transaction[0];
        public TransactionSearchQuery Query { get; set; } = new TransactionSearchQuery();
    }

    public class TransactionSearchQuery
    {
        public int? Count { get; set; }
        public int? Skip { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Resultcount { get; set; }
    }
}
