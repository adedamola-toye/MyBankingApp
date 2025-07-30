using BankingApp.Interfaces;
using BankingApp.Models;

namespace BankingApp.Services
{
    /// <summary>
    /// Provides services for managing financial transactions.
    /// </summary>
    /// <remarks>
    /// This service handles:
    /// <list type="bullet">
    ///   <item><description>Recording transaction history</description></item>
    ///   <item><description>Retrieving transaction records</description></item>
    /// </list>
    /// <para>
    /// Security Features:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Null validation for all input parameters</description></item>
    ///   <item><description>Defensive copying of transaction lists</description></item>
    /// </list>
    /// </remarks>
    public class TransactionService : ITransactionService
    {
        /// <summary>
        /// Records a transaction in the specified account's history.
        /// </summary>
        /// <param name="account">The <see cref="BankAccount"/> to log the transaction to. Must not be null.</param>
        /// <param name="transaction">The <see cref="Transaction"/> to record. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when:
        /// <list type="bullet">
        ///   <item><description><paramref name="account"/> is null</description></item>
        ///   <item><description><paramref name="transaction"/> is null</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// The transaction is added to the account's <see cref="BankAccount.TransactionHistory"/> collection.
        /// </remarks>
        public void LogTransaction(BankAccount? account, Transaction? transaction)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Bank account cannot be null.");
            }

            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            }

            account.TransactionHistory.Add(transaction);
        }

        /// <summary>
        /// Retrieves all transactions for the specified account.
        /// </summary>
        /// <param name="account">The <see cref="BankAccount"/> to retrieve transactions from. Must not be null.</param>
        /// <returns>
        /// A new <see cref="List{Transaction}"/> containing all transactions for the account.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="account"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This method returns a defensive copy of the transaction history to prevent:
        /// </para>
        /// <list type="bullet">
        ///   <item><description>External modification of the original collection</description></item>
        ///   <item><description>Thread-safety issues</description></item>
        /// </list>
        /// </remarks>
        public List<Transaction> GetTransactions(BankAccount? account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Bank account cannot be null.");
            }

            // Defensive copy to prevent external modification
            return [.. account.TransactionHistory];
        }
    }
}