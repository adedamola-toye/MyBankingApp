using BankingApp.Models;
using BankingApp.Helpers;

namespace BankingApp.Helpers
{
    /// <summary>
    /// Provides console-based account selection utilities
    /// </summary>
    public static class AccountSelector
    {
        /// <summary>
        /// Displays a numbered list of accounts and prompts the user to select one
        /// </summary>
        /// <param name="user">The user whose accounts should be displayed</param>
        /// <returns>The selected BankAccount</returns>
        /// <exception cref="ArgumentNullException">Thrown if user is null</exception>
        /// <exception cref="ArgumentException">Thrown if user has no accounts</exception>
        /// <remarks>
        /// The selection menu displays accounts in the format:
        /// [Index]. [AccountType] - [AccountNumber]
        /// Validates input and re-prompts on invalid selections.
        /// </remarks>
        public static BankAccount SelectAccount(User user)
        {
            while (true)
            {
                Console.WriteLine("\nSelect account (or 'C' to cancel):");
                for (int i = 0; i < user.Accounts.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {user.Accounts[i].AccountNo} ({user.Accounts[i].Type})");
                }

                var input = Console.ReadLine();

                if (UserInteraction.ShouldCancel(input!))
                {
                    throw new OperationCanceledException();
                }

                if (int.TryParse(input, out int index) && index > 0 && index <= user.Accounts.Count)
                {
                    return user.Accounts[index - 1];
                }

                Console.WriteLine("❌ Invalid selection");
            }
        }
    }
}