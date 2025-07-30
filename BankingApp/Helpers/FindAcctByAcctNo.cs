using BankingApp.Data;
using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Services;

namespace BankingApp.Helpers
{
    /// <summary>
    /// Provides utility methods for locating bank accounts
    /// </summary>
    public static class AccountFinder
    {
        /// <summary>
        /// Searches all users to find an account by account number
        /// </summary>
        /// <param name="userService">User service instance for accessing account data</param>
        /// <param name="accountNo">Account number to search for</param>
        /// <returns>
        /// Tuple containing:
        /// - Success status (true if found)
        /// - Status message
        /// - Found account (or null if not found)
        /// </returns>
        /// <example>
        /// var result = AccountFinder.FindAcctByAcctNo(userService, "1234567890");
        /// if (result.Success) { /* use result.Account */ }
        /// </example>
        public static (bool Success, string Message, BankAccount? Account) FindAcctByAcctNo(
            IUserService userService, 
            string accountNo)
        {
            foreach (var user in userService.GetAllUsers())
            {
                foreach (var account in user.Accounts)
                {
                    if (account.AccountNo == accountNo)
                    {
                        return (true, "Account found", account);
                    }
                }
            }
            return (false, "Account not found", null);
        }
    }
}