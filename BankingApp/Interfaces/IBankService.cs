using BankingApp.Models;

namespace BankingApp.Interfaces
{
    /// <summary>
    /// Provides banking operations for accounts
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Creates a new bank account for a user
        /// </summary>
        /// <param name="user">Account owner</param>
        /// <param name="type">Type of account to create</param>
        /// <returns>Newly created account</returns>
        BankAccount CreateAccount(User? user, AccountType type);

        /// <summary>
        /// Deposits money into an account
        /// </summary>
        /// <param name="account">Account to deposit into</param>
        /// <param name="amount">Amount to deposit</param>
        /// <returns>Success status and message</returns>
        (bool Success, string Message) Deposit(BankAccount? account, decimal amount);

        /// <summary>
        /// Withdraws money from an account
        /// </summary>
        /// <param name="account">Account to withdraw from</param>
        /// <param name="amount">Amount to withdraw</param>
        /// <returns>Success status and message</returns>
        (bool Success, string Message) Withdraw(BankAccount? account, decimal amount);

        /// <summary>
        /// Transfers money between accounts
        /// </summary>
        /// <param name="sender">Account sending money</param>
        /// <param name="recipient">Account receiving money</param>
        /// <param name="amount">Amount to transfer</param>
        /// <returns>Success status and message</returns>
        (bool Success, string Message) Transfer(BankAccount? sender, BankAccount recipient, decimal amount);

        /// <summary>
        /// Gets all accounts of a specific type for a user
        /// </summary>
        /// <param name="user">Account owner</param>
        /// <param name="type">Type of accounts to retrieve</param>
        /// <returns>List of matching accounts</returns>
        List<BankAccount> GetAccountsByType(User? user, AccountType type);
    }
}