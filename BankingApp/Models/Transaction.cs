using System.Collections.Generic;

namespace BankingApp.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }
    public class Transaction
    {
        public string Id { get; set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get;  private set; }
        public string Description { get; set; }
        
        public BankAccount? OtherPartyAcct { get; set; }
        public TransactionType Type { get; set; }


        public Transaction(decimal amount, string description, BankAccount otherPartyAcct, TransactionType Type)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Amount = amount;
            this.Date = DateTime.Now;
            this.Description = description;
            this.OtherPartyAcct = otherPartyAcc;
            this.Type = type;
        }
    }
}