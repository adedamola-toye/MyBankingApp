using BankingApp.Helpers;
using BankingApp.Interfaces;

namespace BankingApp.Services
{
    /// <summary>
    /// Provides authentication services for user password management.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service handles:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Secure password hashing</description></item>
    ///   <item><description>Password verification</description></item>
    /// </list>
    /// <para>
    /// Security Implementation:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Uses <see cref="HashingUtil"/> for cryptographic operations</description></item>
    ///   <item><description>Implements industry-standard password hashing</description></item>
    ///   <item><description>Never stores plain text passwords</description></item>
    /// </list>
    /// </remarks>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Generates a secure hash of the provided password.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A secure hash string of the password.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="password"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="password"/> is empty or whitespace.</exception>
        /// <remarks>
        /// The hashing algorithm includes:
        /// <list type="bullet">
        ///   <item><description>Salting for rainbow table protection</description></item>
        ///   <item><description>Key stretching for brute-force resistance</description></item>
        /// </list>
        /// </remarks>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            }

            return HashingUtil.HashPassword(password);
        }

        /// <summary>
        /// Verifies if a entered password matches the stored hash.
        /// </summary>
        /// <param name="enteredPassword">The plain text password to verify.</param>
        /// <param name="storedPasswordHash">The previously stored hash to compare against.</param>
        /// <returns>True if the password matches the hash, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="enteredPassword"/> or <paramref name="storedPasswordHash"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when either parameter is empty or whitespace.
        /// </exception>
        /// <remarks>
        /// Uses constant-time comparison to prevent timing attacks.
        /// </remarks>
        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(enteredPassword))
            {
                throw new ArgumentException("Entered password cannot be empty.", nameof(enteredPassword));
            }

            if (string.IsNullOrWhiteSpace(storedPasswordHash))
            {
                throw new ArgumentException("Stored password hash cannot be empty.", nameof(storedPasswordHash));
            }

            return HashingUtil.VerifyPassword(enteredPassword, storedPasswordHash);
        }
    }
}