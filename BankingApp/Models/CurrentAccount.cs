namespace BankingApp.Models
{
    /// <summary>
    /// Represents a current (checking) account that allows deposits, withdrawals, and transfers.
    /// Inherits from <see cref="BankAccount"/> with no interest accrual.
    /// </summary>
    /// <remarks>
    /// Current accounts typically:
    /// <list type="bullet">
    ///   <item><description>Have no monthly interest</description></item>
    ///   <item><description>May allow overdrafts (not implemented here)</description></item>
    /// </list>
    /// </remarks>
    public class CurrentAccount : BankAccount
    {
        /// <summary>
        /// Initializes a new current account for the specified owner.
        /// </summary>
        /// <param name="owner">The account owner. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="owner"/> is null.</exception>
        public CurrentAccount(User owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.Type = AccountType.Current;
        }

        /// <summary>
        /// Deposits the specified amount into the account and records the transaction.
        /// </summary>
        /// <param name="amount">The positive amount to deposit. Must be greater than zero.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        /// </list>
        /// </exception>
        public override void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("❌ Deposit amount must be greater than zero.", nameof(amount));
            }

            decimal oldBalance = this.Balance;
            Balance += amount;
            string description = $"{amount:N0} deposited into your Current Account";
            TransactionHistory.Add(new Transaction(amount, oldBalance, description, null, TransactionType.Deposit, DateTime.Now));
        }

        /// <summary>
        /// Withdraws the specified amount from the account if sufficient funds are available.
        /// </summary>
        /// <param name="amount">The positive amount to withdraw. Must not exceed <see cref="BankAccount.Balance"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        ///   <item><description>Insufficient funds are available</description></item>
        /// </list>
        /// </exception>
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(" ❌ Withdrawal amount must be greater than zero.", nameof(amount));
            }

            if (Balance < amount)
            {
                throw new ArgumentException("❌ Insufficient funds for withdrawal.", nameof(amount));
            }

            decimal oldBalance = this.Balance;
            Balance -= amount;
            string description = $"{amount:N0} withdrawn from your Current Account";
            TransactionHistory.Add(new Transaction(amount, oldBalance, description, null, TransactionType.Withdraw, DateTime.Now));
        }

        /// <summary>
        /// Transfers funds to another account and records transactions in both accounts.
        /// </summary>
        /// <param name="amount">The positive amount to transfer. Must not exceed <see cref="BankAccount.Balance"/>.</param>
        /// <param name="recipientAccount">The destination account. Must not be null.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        ///   <item><description>Insufficient funds are available</description></item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="recipientAccount"/> is null.</exception>
        public override void Transfer(decimal amount, BankAccount recipientAccount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("❌ Transfer amount must be greater than zero.", nameof(amount));
            }

            if (recipientAccount == null)
            {
                throw new ArgumentNullException(nameof(recipientAccount));
            }

            if (Balance < amount)
            {
                throw new ArgumentException("❌ Insufficient funds for transfer.", nameof(amount));
            }

            decimal senderOldBalance = this.Balance;
            Balance -= amount;
            recipientAccount.Credit(amount);

            string recipientName = recipientAccount.Owner?.Username ?? "Unknown";
            string recipientType = recipientAccount.Type.ToString();
            string senderDescription = $"{amount:N0} sent to {recipientName}'s {recipientType} Account";

            string senderName = this.Owner?.Username ?? "Unknown";
            string receiverDescription = $"{amount:N0} received from {senderName}'s {this.Type} Account";

            var timestamp = DateTime.Now;
            // Sender's transaction
            TransactionHistory.Add(new Transaction(
        amount,
        senderOldBalance,
        senderDescription,
        recipientAccount,
        TransactionType.Transfer,  // Add this
        timestamp                 // Add this
    ));

            // Recipient's transaction
            recipientAccount.TransactionHistory.Add(new Transaction(
                amount,
                recipientAccount.Balance - amount,
                receiverDescription,
                this,
                TransactionType.Transfer,  // Add this
                timestamp                  // Add this
            ));
        }
    }
}