using BankingApp.Models;

namespace BankingApp.Interfaces
{
    /// <summary>
    /// Provides user account management and authentication services
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="username">User's login name</param>
        /// <param name="password">User's password</param>
        /// <returns>Tuple containing success status and message</returns>
        (bool Success, string Message) Register(string username, string password);

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="username">User's login name</param>
        /// <param name="password">User's password</param>
        /// <returns>Tuple containing success status, message, and user object if successful</returns>
        (bool Success, string Message, User? User) Login(string username, string password);

        /// <summary>
        /// Retrieves a user by username
        /// </summary>
        /// <param name="username">User's login name to search for</param>
        /// <returns>User object if found, otherwise null</returns>
        User? GetUser(string username);

        /// <summary>
        /// Saves all user data to persistent storage
        /// </summary>
        void Save();

        /// <summary>
        /// Gets all registered users
        /// </summary>
        /// <returns>List of all users in the system</returns>
        List<User> GetAllUsers();
    }
}