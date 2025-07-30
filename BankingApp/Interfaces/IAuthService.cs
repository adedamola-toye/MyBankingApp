namespace BankingApp.Interfaces
{
    /// <summary>
    /// Handles password security operations
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Creates a secure hash from a password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>Hashed password string</returns>
        string HashPassword(string password);

        /// <summary>
        /// Checks if a password matches its hashed version
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <param name="hashedPassword">Stored hashed password</param>
        /// <returns>True if password matches, false otherwise</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}