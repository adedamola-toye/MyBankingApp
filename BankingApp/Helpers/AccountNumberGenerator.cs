namespace BankingApp.Helpers
{
    /// <summary>
    /// Generates unique bank account numbers
    /// </summary>
    public static class AccountNumberGenerator
    {
        /// <summary>
        /// Generates a new 10-digit account number based on current timestamp
        /// </summary>
        /// <returns>10-character account number string</returns>
        /// <remarks>
        /// Uses the last 10 digits of the current DateTime ticks value.
        /// This provides a simple but reasonably unique identifier for demo purposes.
        /// For production systems, consider a more robust solution.
        /// </remarks>
        public static string Generate()
        {
            return DateTime.Now.Ticks.ToString().Substring(0, 10);
        }
    }
}