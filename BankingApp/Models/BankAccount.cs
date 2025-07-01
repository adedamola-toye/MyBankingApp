
using System.Collections.Generic;

        

namespace BankingApp.Models
{
    public enum AccountType
        {
            Savings,
            Current
        }
    public abstract class BankAccount
    {
        public string AccountNo { get; set; }
        public decimal Balance { get; protected set; }

        public DateTime DateCreated { get; set; }

        public User Owner { get; set; }

        public List<Transaction> TransactionHistory { get; private set; }

        public AccountType Type { get; set; }


        public BankAccount()
        {
            this.AccountNo = Helpers.AccountNumberGenerator.Generate();
            this.Balance = 0;
            this.DateCreated = DateTime.Now;
            this.TransactionHistory = new List<Transaction>();

        }

        public abstract void Deposit(decimal amount);
        public abstract void Withdraw(decimal amount);
        public abstract void Transfer(decimal amount, BankAccount recipientAccount);
    }
}