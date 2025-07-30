using System;
using System.Collections.Generic;
using BankingApp.Models;
using BankingApp.Services;
using Xunit;

namespace BankingApp.Tests
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly User _user;
        private readonly BankAccount _account;

        public TransactionServiceTests()
        {
            _transactionService = new TransactionService();
            _user = new User("testuser", "hash");
            _account = new SavingsAccount(_user) { Owner = _user };
        }

        [Fact]
        public void LogTransaction_AddsTransactionToAccount()
        {
            var transaction = new Transaction(100, "Deposit", null, TransactionType.Deposit);

            _transactionService.LogTransaction(_account, transaction);
            Assert.Contains(transaction, _account.TransactionHistory);
        }

        [Fact]
        public void LogTransaction_NullAccount_ThrowsException()
        {
            var transaction = new Transaction(100, "Deposit", null, TransactionType.Deposit);
            Assert.Throws<ArgumentNullException>(() => _transactionService.LogTransaction(null, transaction));
        }

        [Fact]
        public void LogTransaction_NullTransaction_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _transactionService.LogTransaction(_account, null));
        }

        [Fact]
        public void GetTransactions_ReturnsAllTransactions()
        {
            var t1 = new Transaction(50, "Deposit", null, TransactionType.Deposit);
            var t2 = new Transaction(20, "Withdrawal", null, TransactionType.Withdraw);

            _account.TransactionHistory.Add(t1);
            _account.TransactionHistory.Add(t2);
            var result = _transactionService.GetTransactions(_account);
            Assert.Equal(2, result.Count);
            Assert.Contains(t1, result);
            Assert.Contains(t2, result);
        }

        [Fact]
        public void GetTransactions_NullAccount_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _transactionService.GetTransactions(null));
        }

        [Fact]
        public void GetTransactions_ReturnsDefensiveCopy()
        {
            var t1 = new Transaction(10, "Deposit", null, TransactionType.Deposit);
            _account.TransactionHistory.Add(t1);
            var result = _transactionService.GetTransactions(_account);
            result.Clear();
            Assert.Single(_account.TransactionHistory);
        }
    }
}
