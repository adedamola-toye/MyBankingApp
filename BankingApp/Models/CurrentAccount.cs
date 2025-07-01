
namespace BankingApp.Models
{
    public class CurrentAccount : BankAccount
    {
        
        public override void Deposit(decimal amount)
        {
            Balance += amount;
            string description = $"{amount:N0} deposited into your Current Account";
            TransactionHistory.Add(new Transaction(amount, description, null, TransactionType.Deposit));
        }

        public override void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                string description = $"{amount:N0} withdrawn from your Current Account";
                TransactionHistory.Add(new Transaction(amount, description, null, TransactionType.Withdraw));
            }
            else
            {
                throw new ArgumentException("Insufficient Funds");
            }
        }

        public override void Transfer(decimal amount, BankAccount recipientAccount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                recipientAccount.Balance += amount;


                string senderDescription = $"{amount:N0} sent to {recipientAccount.Owner.Username}'s {recipientAccount.Type} Account";

                //sender transaction
                string receiverDescription = $"{amount:N0} received from{Owner.Username}'s Current Account";

                //sender sees transaction
                TransactionHistory.Add(new Transaction(amount, senderDescription, recipientAccount, TransactionType.Transfer));

                //recipient sees transaction
                recipientAccount.TransactionHistory.Add(new Transaction(amount, receiverDescription, this, TransactionType.Transfer));

            }
            else
            {
                throw new ArgumentException("Insufficient Funds");
            }
        }

        public CurrentAccount(User owner)
        {
            this.Owner = owner;
            this.Type = AccountType.Current;
        }

        
    }
}