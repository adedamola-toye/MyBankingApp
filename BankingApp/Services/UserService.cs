using BankingApp.Models;
using BankingApp.Interfaces;

namespace BankingApp.Services
{
    /// <summary>
    /// Provides user management services including registration, authentication, and user data access.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service handles:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>User registration and validation</description></item>
    ///   <item><description>User authentication (login)</description></item>
    ///   <item><description>User data persistence</description></item>
    ///   <item><description>User information retrieval</description></item>
    /// </list>
    /// <para>
    /// Security Features:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Password hashing via <see cref="IAuthService"/></description></item>
    ///   <item><description>Input validation for credentials</description></item>
    ///   <item><description>In-memory user caching with file persistence</description></item>
    /// </list>
    /// </remarks>
    public class UserService : IUserService
    {
        private readonly IDataStore<string, User> _dataStore;
        private readonly Dictionary<string, User> _users;
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="dataStore">The data store for user persistence.</param>
        /// <param name="authService">The authentication service for password handling.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="dataStore"/> or <paramref name="authService"/> is null.
        /// </exception>
        /// <remarks>
        /// Automatically loads user data from the data store into memory during initialization.
        /// </remarks>
        public UserService(IDataStore<string, User> dataStore, IAuthService authService)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _users = _dataStore.Load();
        }

        /// <summary>
        /// Persists all user data to the underlying data store.
        /// </summary>
        /// <remarks>
        /// Writes the current in-memory user collection to persistent storage.
        /// </remarks>
        public void Save()
        {
            _dataStore.Save(_users);
        }

        /// <summary>
        /// Registers a new user with the system.
        /// </summary>
        /// <param name="username">The desired username. Must not be empty or whitespace.</param>
        /// <param name="password">The plain-text password. Must not be empty or whitespace.</param>
        /// <returns>
        /// A tuple containing:
        /// <list type="table">
        ///   <item><term>Success</term><description>True if registration succeeded</description></item>
        ///   <item><term>Message</term><description>Status message or error description</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// <para>
        /// Registration process:
        /// </para>
        /// <list type="number">
        ///   <item><description>Validates input parameters</description></item>
        ///   <item><description>Checks for username availability</description></item>
        ///   <item><description>Hashes the password</description></item>
        ///   <item><description>Creates and stores the new user</description></item>
        ///   <item><description>Persists changes automatically</description></item>
        /// </list>
        /// </remarks>
        public (bool Success, string Message) Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Username and password are required.");
            }

            bool usernameTaken = _users.Values.Any(user => user.Username == username);
            if (usernameTaken)
            {
                return (false, "Username already exists.");
            }

            try
            {
                var hashedPassword = _authService.HashPassword(password);
                var user = new User(username, hashedPassword);

                _users.Add(user.UserId, user);
                _dataStore.Save(_users);
                return (true, "Registration successful");
            }
            catch (Exception ex)
            {
                return (false, $"Error during registration: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="username">The username to authenticate.</param>
        /// <param name="password">The plain-text password to verify.</param>
        /// <returns>
        /// A tuple containing:
        /// <list type="table">
        ///   <item><term>Success</term><description>True if authentication succeeded</description></item>
        ///   <item><term>Message</term><description>Status message or error description</description></item>
        ///   <item><term>User</term><description>The authenticated user if successful, otherwise null</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// <para>
        /// Authentication process:
        /// </para>
        /// <list type="number">
        ///   <item><description>Validates input parameters</description></item>
        ///   <item><description>Verifies username exists</description></item>
        ///   <item><description>Verifies password matches stored hash</description></item>
        /// </list>
        /// </remarks>
        public (bool Success, string Message, User? User) Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Username and password are required.", null);
            }

            var user = _users.Values.FirstOrDefault(user => user.Username == username);
            if (user == null)
            {
                return (false, "User does not exist.", null);
            }
            if (!_authService.VerifyPassword(password, user.PasswordHash))
            {
                return (false, "Incorrect password.", null);
            }
            return (true, "Login successful.", user);
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>
        /// The <see cref="User"/> if found, otherwise null.
        /// </returns>
        /// <remarks>
        /// Performs a case-sensitive search of the username.
        /// </remarks>
        public User? GetUser(string username)
        {
            return _users.Values.FirstOrDefault(user => user.Username == username);
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>
        /// A <see cref="List{User}"/> containing all registered users.
        /// </returns>
        /// <remarks>
        /// Returns a defensive copy of the user collection to prevent modification.
        /// </remarks>
        public List<User> GetAllUsers()
        {
            return [.. _users.Values];
        }
    }
}