using System.Collections.Generic;

namespace BankingApp.Models
{
    /// <summary>
    /// Specifies the type of financial transaction.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Funds being added to an account.
        /// </summary>
        Deposit,

        /// <summary>
        /// Funds being removed from an account.
        /// </summary>
        Withdraw,

        /// <summary>
        /// Funds being moved between accounts.
        /// </summary>
        Transfer
    }

    /// <summary>
    /// Represents a financial transaction in the banking system.
    /// </summary>
    /// <remarks>
    /// This class records all details of monetary movements including:
    /// <list type="bullet">
    ///   <item><description>The transaction amount</description></item>
    ///   <item><description>Timestamp</description></item>
    ///   <item><description>Description of the transaction</description></item>
    ///   <item><description>Related account (for transfers)</description></item>
    /// </list>
    /// </remarks>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the unique transaction identifier.
        /// </summary>
        /// <value>
        /// Auto-generated GUID string that uniquely identifies the transaction.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets the monetary amount involved in the transaction.
        /// </summary>
        /// <value>
        /// Positive decimal number representing the transaction value.
        /// </value>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the date and time when the transaction occurred.
        /// </summary>
        /// <value>
        /// DateTime set to when the transaction was created.
        /// </value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets or sets the human-readable description of the transaction.
        /// </summary>
        /// <value>
        /// String explaining the transaction purpose in natural language.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the related account for transfer transactions.
        /// </summary>
        /// <value>
        /// <see cref="BankAccount"/> involved in the transaction (for transfers), or null for deposits/withdrawals.
        /// </value>
        public BankAccount? OtherPartyAcct { get; set; }

        /// <summary>
        /// Gets or sets the classification of the transaction.
        /// </summary>
        /// <value>
        /// <see cref="TransactionType"/> enum value specifying the transaction category.
        /// </value>
        public TransactionType Type { get; set; }

    /// <summary>
    ///  
    /// </summary>
        public decimal BalanceBeforeTransaction { get; }

        /// <summary>
        /// Gets the account balance immediately after this transaction was processed.
        /// </summary>
        /// <value>
        /// Decimal value representing the running balance post-transaction.
        /// </value>
        /// <remarks>
        /// This snapshot helps reconstruct account history and provides context for
        /// financial auditing. It's set automatically during transaction processing.
        /// </remarks>
        public decimal BalanceAfterTransaction { get; }




        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="amount">The positive monetary value of the transaction.</param>
        /// <param name="currentBalance">The account balance BEFORE processing this transaction.</param>

        /// <param name="description">Human-readable explanation of the transaction.</param>
        /// <param name="otherPartyAcct">
        /// The other <see cref="BankAccount"/> involved (for transfers), or null for deposits/withdrawals.
        /// </param>
        /// <param name="type">The <see cref="TransactionType"/> classification.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="amount"/> is zero or negative.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="description"/> is null or empty.
        /// </exception>
        /// <param name="timestamp">
        /// Optional DateTime to set the transaction date. Defaults to current time if null.
        /// </param>
        /// /// <remarks>
        /// This constructor sets the transaction ID, amount, date, description, related account,
        /// type, and calculates the balance after the transaction. 
        /// /// It ensures all required fields are properly initialized and validated.
        ///</remarks>
        public Transaction(decimal amount, decimal currentBalance, string description, BankAccount? otherPartyAcct, TransactionType type, DateTime? timestamp = null)
        {

            {
                if (amount <= 0)
                {
                    throw new ArgumentException("Transaction amount must be positive.", nameof(amount));
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentNullException(nameof(description), "Transaction description cannot be empty.");
                }

                this.Id = Guid.NewGuid().ToString();
                this.Amount = amount;
                this.Description = description;
                this.OtherPartyAcct = otherPartyAcct;
                this.Type = type;
                this.BalanceBeforeTransaction = currentBalance;
                this.Date = timestamp ?? DateTime.Now; // Use provided date or current time
                this.BalanceAfterTransaction = type switch
                {
                    TransactionType.Deposit => currentBalance + amount,
                    TransactionType.Withdraw => currentBalance - amount,
                    TransactionType.Transfer => currentBalance - amount, // For sender account
                    _ => throw new ArgumentOutOfRangeException(nameof(type))
                };
            }
        }
    }
}