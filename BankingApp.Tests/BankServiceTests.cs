using System;
using System.Collections.Generic;
using BankingApp.Models;
using BankingApp.Services;
using BankingApp.Interfaces;
using Moq;
using Xunit;

namespace BankingApp.Tests
{
    public class BankServiceTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly BankService _bankService;
        private readonly User _user;

        public BankServiceTests()
        {
            _mockUserService = new Mock<IUserService>();
            _bankService = new BankService(_mockUserService.Object);
            _user = new User("testuser", "hash") { Accounts = new List<BankAccount>() };
        }

        [Fact]
        public void CreateAccount_SavingsAccount_Success()
        {
            var account = _bankService.CreateAccount(_user, AccountType.Savings);
            Assert.NotNull(account);
            Assert.IsType<SavingsAccount>(account);
            Assert.Contains(account, _user.Accounts);
            _mockUserService.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void CreateAccount_CurrentAccount_Success()
        {
            var account = _bankService.CreateAccount(_user, AccountType.Current);
            Assert.NotNull(account);
            Assert.IsType<CurrentAccount>(account);
            Assert.Contains(account, _user.Accounts);
            _mockUserService.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void CreateAccount_NullUser_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _bankService.CreateAccount(null, AccountType.Savings));
        }

        [Fact]
        public void Deposit_Success()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            var result = _bankService.Deposit(account, 100);
            Assert.True(result.Success);
            Assert.Equal("Deposit successful.", result.Message);
            _mockUserService.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void Deposit_NullAccount_ReturnsError()
        {
            var result = _bankService.Deposit(null, 100);
            Assert.False(result.Success);
            Assert.Equal("Account cannot be null.", result.Message);
        }

        [Fact]
        public void Deposit_NegativeAmount_ReturnsError()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            var result = _bankService.Deposit(account, -50);
            Assert.False(result.Success);
            Assert.Equal("Deposit amount must be greater than zero.", result.Message);
        }

        [Fact]
        public void Withdraw_Success()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            account.Deposit(200);
            var result = _bankService.Withdraw(account, 100);
            Assert.True(result.Success);
            Assert.Equal("Withdrawal successful.", result.Message);
            _mockUserService.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void Withdraw_NullAccount_ReturnsError()
        {
            var result = _bankService.Withdraw(null, 100);
            Assert.False(result.Success);
            Assert.Equal("Account cannot be null.", result.Message);
        }

        [Fact]
        public void Withdraw_NegativeAmount_ReturnsError()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            var result = _bankService.Withdraw(account, -50);
            Assert.False(result.Success);
            Assert.Equal("Withdrawal amount must be greater than zero.", result.Message);
        }

        [Fact]
        public void Transfer_Success()
        {
            var sender = new SavingsAccount(_user) { Owner = _user };
            var recipient = new SavingsAccount(_user) { Owner = _user };
            sender.Deposit(300);
            var result = _bankService.Transfer(sender, recipient, 100);
            Assert.True(result.Success);
            Assert.Equal("Transfer successful.", result.Message);
            _mockUserService.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void Transfer_NullSenderOrRecipient_ReturnsError()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            var result1 = _bankService.Transfer(null, account, 100);
            var result2 = _bankService.Transfer(account, null, 100);
            Assert.False(result1.Success);
            Assert.False(result2.Success);
            Assert.Equal("Sender and recipient accounts must be provided.", result1.Message);
            Assert.Equal("Sender and recipient accounts must be provided.", result2.Message);
        }

        [Fact]
        public void Transfer_SameAccount_ReturnsError()
        {
            var account = new SavingsAccount(_user) { Owner = _user };
            var result = _bankService.Transfer(account, account, 100);
            Assert.False(result.Success);
            Assert.Equal("Cannot transfer to the same account.", result.Message);
        }

        [Fact]
        public void Transfer_NegativeAmount_ReturnsError()
        {
            var sender = new SavingsAccount(_user) { Owner = _user };
            var recipient = new SavingsAccount(_user) { Owner = _user };
            var result = _bankService.Transfer(sender, recipient, -100);
            Assert.False(result.Success);
            Assert.Equal("Transfer amount must be greater than zero.", result.Message);
        }

        [Fact]
        public void GetAccountsByType_ReturnsCorrectAccounts()
        {
            var savings = new SavingsAccount(_user) { Owner = _user };
            var current = new CurrentAccount(_user) { Owner = _user };
            _user.Accounts.Add(savings);
            _user.Accounts.Add(current);
            var result = _bankService.GetAccountsByType(_user, AccountType.Savings);
            Assert.Single(result);
            Assert.Contains(savings, result);
        }

        [Fact]
        public void GetAccountsByType_NullUser_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _bankService.GetAccountsByType(null, AccountType.Savings));
        }
    }
}
