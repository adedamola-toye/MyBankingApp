using System.Security.Cryptography;
using System.Text;

namespace BankingApp.Helpers
{
    /// <summary>
    /// Provides cryptographic hashing utilities for password security
    /// </summary>
    /// <remarks>
    /// WARNING: For educational purposes only. In production systems,
    /// use specialized password hashing algorithms like PBKDF2, BCrypt or Argon2.
    /// </remarks>
    public static class HashingUtil
    {
        /// <summary>
        /// Creates a cryptographic hash of a password using SHA-256
        /// </summary>
        /// <param name="password">The plain-text password to hash</param>
        /// <returns>Base64-encoded hash string</returns>
        /// <exception cref="ArgumentNullException">Thrown if password is null</exception>
        /// <remarks>
        /// Note: This basic implementation lacks:
        /// - Salt (vulnerable to rainbow table attacks)
        /// - Key stretching (vulnerable to brute force)
        /// - Modern password hashing algorithms
        /// </remarks>
        public static string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        /// <param name="enteredPassword">Password to verify</param>
        /// <param name="storedHash">Previously stored hash to compare against</param>
        /// <returns>True if passwords match, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if either parameter is null</exception>
        /// <remarks>
        /// Uses simple hash comparison. In production, use constant-time comparison
        /// to prevent timing attacks.
        /// </remarks>
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (enteredPassword == null)
                throw new ArgumentNullException(nameof(enteredPassword));
            if (storedHash == null)
                throw new ArgumentNullException(nameof(storedHash));

            string enteredPasswordHashed = HashPassword(enteredPassword);
            return enteredPasswordHashed == storedHash;
        }
    }
}