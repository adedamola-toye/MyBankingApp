using Newtonsoft.Json;


namespace BankingApp.Models
{
    /// <summary>
    /// Represents a savings account that provides interest earnings and has transfer limits.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Savings accounts typically have:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Interest earning capabilities (not shown in this implementation)</description></item>
    ///   <item><description>Withdrawal and transfer limits (max 200,000 per transaction)</description></item>
    ///   <item><description>Higher interest rates than current accounts</description></item>
    /// </list>
    /// </remarks>
    /// 

    public class SavingsAccount : BankAccount
    {
        /// <summary>
        /// Maximum allowed transfer/withdrawal amount for savings accounts.
        /// </summary>
        /// <value>
        /// Constant value of 200,000 representing the transaction limit.
        /// </value>
        private const decimal MaxTransferLimit = 200000;

        /// <summary>
        /// Initializes a new savings account for the specified owner.
        /// </summary>
        /// <param name="owner">The <see cref="User"/> who owns this account. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="owner"/> is null.</exception>
        public SavingsAccount(User owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.Type = AccountType.Savings;
        }

        /// <summary>
        /// Protected constructor for JSON deserialization
        /// </summary>
        [JsonConstructor]
        protected SavingsAccount(string accountNo, DateTime dateCreated, List<Transaction> transactionHistory, AccountType type)
            : base(accountNo, dateCreated, transactionHistory)
        {
            this.Type = type;
        }


        /// <summary>
        /// Deposits the specified amount into the savings account.
        /// </summary>
        /// <param name="amount">The positive amount to deposit. Must be greater than zero.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// Records the transaction in <see cref="BankAccount.TransactionHistory"/>.
        /// </remarks>
        public override void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.", nameof(amount));
            }

            decimal oldBalance = this.Balance;
            Balance += amount;
            string description = $"{amount:N0} deposited into your Savings Account";
            TransactionHistory.Add(new Transaction(
        amount,
        oldBalance,
        $"{amount:N0} deposited",
        null,
        TransactionType.Deposit,
    
        DateTime.Now // Explicit timestamp
    ));
        }

        /// <summary>
        /// Withdraws the specified amount from the savings account.
        /// </summary>
        /// <param name="amount">The positive amount to withdraw. Must not exceed <see cref="MaxTransferLimit"/> or available balance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> exceeds <see cref="MaxTransferLimit"/></description></item>
        ///   <item><description>Insufficient funds are available</description></item>
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// Savings accounts have stricter withdrawal limits than current accounts.
        /// </remarks>
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.", nameof(amount));
            }

            if (amount > MaxTransferLimit)
            {
                throw new ArgumentException(
                    $"Withdrawal limit exceeded for Savings Account. Maximum allowed: {MaxTransferLimit:N0}",
                    nameof(amount));
            }

            if (Balance < amount)
            {
                throw new ArgumentException("Insufficient funds.", nameof(amount));
            }

            decimal oldBalance = this.Balance;
            Balance -= amount;
            string description = $"{amount:N0} withdrawn from your Savings Account";
            TransactionHistory.Add(new Transaction(
        amount,
        oldBalance,
        description,
        null,
        TransactionType.Withdraw,
        DateTime.Now // Add timestamp
    ));
        }

        /// <summary>
        /// Transfers funds to another account while respecting savings account limits.
        /// </summary>
        /// <param name="amount">The positive amount to transfer. Must not exceed <see cref="MaxTransferLimit"/>.</param>
        /// <param name="recipientAccount">The destination <see cref="BankAccount"/>. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="recipientAccount"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="amount"/> exceeds <see cref="MaxTransferLimit"/></description></item>
        ///   <item><description>Insufficient funds are available</description></item>
        ///   <item><description><paramref name="amount"/> is zero or negative</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// Records transactions in both the sender's and recipient's transaction histories.
        /// </remarks>
        public override void Transfer(decimal amount, BankAccount recipientAccount)
        {
            if (recipientAccount == null)
            {
                throw new ArgumentNullException(nameof(recipientAccount), "Recipient account cannot be null.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero.", nameof(amount));
            }

            if (amount > MaxTransferLimit)
            {
                throw new ArgumentException(
                    $"Transfer limit exceeded for Savings Account. Maximum allowed: {MaxTransferLimit:N0}",
                    nameof(amount));
            }

            if (Balance < amount)
            {
                throw new ArgumentException("Insufficient funds.", nameof(amount));
            }
            decimal senderOldBalance = this.Balance;
            Balance -= amount;
            recipientAccount.Credit(amount);

            string senderName = recipientAccount.Owner?.Username ?? "Unknown";
            string recipientAccountType = recipientAccount.Type.ToString();
            string senderDescription = $"{amount:N0} sent to {senderName}'s {recipientAccountType} Account";

            string receiverName = this.Owner?.Username ?? "Unknown";
            string receiverDescription = $"{amount:N0} received from {receiverName}'s {this.Type} Account";
            
            var timestamp = DateTime.Now;

            // Sender's transaction
            TransactionHistory.Add(new Transaction(
        amount,
        senderOldBalance,
        senderDescription,
        recipientAccount,
        TransactionType.Transfer,
        timestamp
    ));

    // Recipient's transaction
    recipientAccount.TransactionHistory.Add(new Transaction(
        amount,
        recipientAccount.Balance - amount, // Old balance before credit
        receiverDescription,
        this,
        TransactionType.Transfer,
        timestamp
    ));
        }
    }
}