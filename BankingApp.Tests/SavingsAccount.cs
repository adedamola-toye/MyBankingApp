using Xunit;
using BankingApp.Models;

namespace BankingApp.Tests
{
    public class SavingsAccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            //Arrange - set up
            var user = new User("testUser", "testPassword123");
            var savingsAccount = new SavingsAccount(user) { Owner = user };

            //Act - actual action
            savingsAccount.Deposit(1000);

            //Assert - verify
            Assert.Equal(1000, savingsAccount.Balance);
        }

        [Fact]
        public void Deposit_ShouldAddTransactionToHistory()
        {
            //Arrange
            var user = new User("testUser", "testPassword123");
            var savingsAccount = new SavingsAccount(user) { Owner = user };

            //Act
            savingsAccount.Deposit(1000);

            //Assert
            Assert.Single(savingsAccount.TransactionHistory); //ensure only one transaction was added to the transaction history list

            var transaction = savingsAccount.TransactionHistory[0];
            Assert.Equal(1000, transaction.Amount);
            Assert.Equal(TransactionType.Deposit, transaction.Type);
            Assert.Equal("1,000 deposited into your Savings Account", transaction.Description);
            Assert.Null(transaction.OtherPartyAcct);
        }


        [Fact]
        public void Withdraw_ShouldDecreaseBalance_WhenSufficientFunds()
        {
            //Arrange
            var user = new User("testUser", "testPassword123");
            var savingsAccount = new SavingsAccount(user) { Owner = user };

            //Act
            savingsAccount.Deposit(10000);
            savingsAccount.Withdraw(5000);

            //Assert
            Assert.Equal(5000, savingsAccount.Balance);
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenAmountExceedsLimit()
        {
            //Arrange
            var user = new User("testUser", "testPassword123");
            var savingsAccount = new SavingsAccount(user) { Owner = user };

            //Act
            savingsAccount.Deposit(500000);


            //Assert
            var ex = Assert.Throws<ArgumentException>(() => savingsAccount.Withdraw(300000));
            Assert.Equal("Withdrawal limit exceeded for Savings Account.", ex.Message);

        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenBalanceIsTooLow()
        {
            var user = new User("testUser", "testPassword123");
            var savingsAccount = new SavingsAccount(user) { Owner = user };


            savingsAccount.Deposit(5000);

            var ex = Assert.Throws<ArgumentException>(() => savingsAccount.Withdraw(10000));
            Assert.Equal("Insufficient funds", ex.Message);
        }

        [Fact]
        public void Transfer_ShouldDecreaseSenderBalanceAndIncreaseRecipientBalance()
        {
            var user1 = new User("testUser1", "testPassword123");
            var senderAccount = new SavingsAccount(user1) { Owner = user1 };

            var user2 = new User("testUser2", "testPassword1234");
            var recipientAccount = new SavingsAccount(user2) { Owner = user2 };

            senderAccount.Deposit(25000);
            senderAccount.Transfer(10000, recipientAccount);

            Assert.Equal(15000, senderAccount.Balance);
            Assert.Equal(10000, recipientAccount.Balance);
            Assert.Single(senderAccount.TransactionHistory);
            Assert.Single(recipientAccount.TransactionHistory);

            var senderTransaction = senderAccount.TransactionHistory[0];
            var recipientTransaction = recipientAccount.TransactionHistory[0];

            Assert.Equal(10000, senderTransaction.Amount);
            Assert.Equal(TransactionType.Transfer, senderTransaction.Type);
            Assert.Equal("10,000 sent to testUser2's Savings Account", senderTransaction.Description);
            Assert.Equal(recipientAccount, senderTransaction.OtherPartyAcct);

            //Receiver checks
            Assert.Equal(10000, recipientTransaction.Amount);
            Assert.Equal(TransactionType.Transfer, recipientTransaction.Type);
            Assert.Equal("10,000 received from testUser1's Savings Account", recipientTransaction.Description);
            Assert.Equal(senderAccount, recipientTransaction.OtherPartyAcct);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenAmountExceedsTransferLimit()
        {
            var user1 = new User("senderUser", "password123");
            var senderAccount = new SavingsAccount(user1) { Owner = user1 };

            var user2 = new User("recipientUser", "password456");
            var recipientAccount = new SavingsAccount(user2) { Owner = user2 };

            senderAccount.Deposit(300000);

            var ex = Assert.Throws<ArgumentException>(() => senderAccount.Transfer(250000, recipientAccount));
            Assert.Equal("Transfer limit exceeded for Savings Account", ex.Message);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenSenderBalanceIsInsufficient()
        {
            var user1 = new User("senderUser", "password123");
            var senderAccount = new SavingsAccount(user1) { Owner = user1 };

            var user2 = new User("recipientUser", "password456");
            var recipientAccount = new SavingsAccount(user2) { Owner = user2 };

            senderAccount.Deposit(200000);

            var ex = Assert.Throws<ArgumentException>(() => senderAccount.Transfer(250000, recipientAccount));
            Assert.Equal("Insufficient Funds", ex.Message);
        }



    }

}



/* Transfer should throw when amount exceeds transfer limit

Transfer should throw when sender balance is insufficient

Transfer should record transaction in sender's history

Transfer should record transaction in recipient's history

 */