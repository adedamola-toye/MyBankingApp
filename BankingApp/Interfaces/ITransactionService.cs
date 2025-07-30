using BankingApp.Models;

namespace BankingApp.Interfaces
{
    /// <summary>
    /// Provides services for managing financial transactions
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Records a transaction in an account's history
        /// </summary>
        /// <param name="account">Account to log transaction for</param>
        /// <param name="transaction">Transaction details to record</param>
        void LogTransaction(BankAccount account, Transaction transaction);

        /// <summary>
        /// Retrieves all transactions for an account
        /// </summary>
        /// <param name="account">Account to get transactions for</param>
        /// <returns>List of transactions ordered by date</returns>
        List<Transaction> GetTransactions(BankAccount account);
    }
}