using Xunit;
using BankingApp.Models;

namespace BankingApp.Tests
{
    public class CurrentAccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var user = new User("testUser", "testPassword123");
            var currentAccount = new CurrentAccount(user) { Owner = user };

            currentAccount.Deposit(2000);

            Assert.Equal(2000, currentAccount.Balance);
        }

        [Fact]
        public void Deposit_ShouldAddTransactionToHistory()
        {
            var user = new User("testUser", "testPassword123");
            var currentAccount = new CurrentAccount(user) { Owner = user };

            currentAccount.Deposit(2000);

            Assert.Single(currentAccount.TransactionHistory);
            var transaction = currentAccount.TransactionHistory[0];
            Assert.Equal(2000, transaction.Amount);
            Assert.Equal(TransactionType.Deposit, transaction.Type);
            Assert.Equal("2,000 deposited into your Current Account", transaction.Description);
            Assert.Null(transaction.OtherPartyAcct);
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance_WhenSufficientFunds()
        {
            var user = new User("testUser", "testPassword123");
            var currentAccount = new CurrentAccount(user) { Owner = user };

            currentAccount.Deposit(5000);
            currentAccount.Withdraw(3000);

            Assert.Equal(2000, currentAccount.Balance);
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenBalanceIsTooLow()
        {
            var user = new User("testUser", "testPassword123");
            var currentAccount = new CurrentAccount(user) { Owner = user };

            currentAccount.Deposit(1000);

            var ex = Assert.Throws<ArgumentException>(() => currentAccount.Withdraw(2000));
            Assert.Equal("Insufficient Funds", ex.Message);
        }

        [Fact]
        public void Transfer_ShouldDecreaseSenderBalanceAndIncreaseRecipientBalance()
        {
            var user1 = new User("user1", "pass1");
            var senderAccount = new CurrentAccount(user1) { Owner = user1 };

            var user2 = new User("user2", "pass2");
            var recipientAccount = new CurrentAccount(user2) { Owner = user2 };

            senderAccount.Deposit(10000);
            senderAccount.Transfer(4000, recipientAccount);

            Assert.Equal(6000, senderAccount.Balance);
            Assert.Equal(4000, recipientAccount.Balance);

            Assert.Single(senderAccount.TransactionHistory);
            Assert.Single(recipientAccount.TransactionHistory);

            var senderTransaction = senderAccount.TransactionHistory[0];
            var recipientTransaction = recipientAccount.TransactionHistory[0];

            Assert.Equal(4000, senderTransaction.Amount);
            Assert.Equal(TransactionType.Transfer, senderTransaction.Type);
            Assert.Equal("4,000 sent to user2's Current Account", senderTransaction.Description);
            Assert.Equal(recipientAccount, senderTransaction.OtherPartyAcct);

            Assert.Equal(4000, recipientTransaction.Amount);
            Assert.Equal(TransactionType.Transfer, recipientTransaction.Type);
            Assert.Equal("4,000 received from user1's Current Account", recipientTransaction.Description);
            Assert.Equal(senderAccount, recipientTransaction.OtherPartyAcct);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenSenderBalanceIsInsufficient()
        {
            var user1 = new User("user1", "pass1");
            var senderAccount = new CurrentAccount(user1) { Owner = user1 };

            var user2 = new User("user2", "pass2");
            var recipientAccount = new CurrentAccount(user2) { Owner = user2 };

            senderAccount.Deposit(1000);

            var ex = Assert.Throws<ArgumentException>(() => senderAccount.Transfer(2000, recipientAccount));
            Assert.Equal("Insufficient Funds", ex.Message);
        }
    }
}