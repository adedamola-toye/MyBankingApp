using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Services;

namespace BankingApp.Services
{
    /// <summary>
    /// Provides core banking operations for account management and transactions.
    /// </summary>
    /// <remarks>
    /// This service acts as the main facade for banking operations including:
    /// <list type="bullet">
    ///   <item><description>Account creation</description></item>
    ///   <item><description>Deposit/withdrawal processing</description></item>
    ///   <item><description>Fund transfers between accounts</description></item>
    ///   <item><description>Account querying</description></item>
    /// </list>
    /// <para>
    /// All operations automatically persist changes via <see cref="IUserService"/>.
    /// </para>
    /// </remarks>
    public class BankService : IBankService
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankService"/> class.
        /// </summary>
        /// <param name="userService">The user service dependency for data persistence.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="userService"/> is null.</exception>
        public BankService(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Creates a new bank account of the specified type for the given user.
        /// </summary>
        /// <param name="user">The account owner. Must not be null.</param>
        /// <param name="type">The type of account to create.</param>
        /// <returns>The newly created <see cref="BankAccount"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="type"/> is invalid.</exception>
        /// <remarks>
        /// The created account is automatically:
        /// <list type="bullet">
        ///   <item><description>Added to the user's account list</description></item>
        ///   <item><description>Persisted via <see cref="IUserService"/></description></item>
        /// </list>
        /// </remarks>
        public BankAccount CreateAccount(User? user, AccountType type)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            BankAccount newAccount = type switch
            {
                AccountType.Savings =>  new SavingsAccount(user) { Owner = user },
                AccountType.Current => new CurrentAccount(user){Owner = user},
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid account type specified.")
            };

            user.Accounts.Add(newAccount);
            _userService.Save();
            return newAccount;
        }

        /// <summary>
        /// Processes a deposit transaction into the specified account.
        /// </summary>
        /// <param name="account">The target account. Must not be null.</param>
        /// <param name="amount">The positive amount to deposit.</param>
        /// <returns>
        /// A tuple where:
        /// <list type="table">
        ///   <item><term>Success</term><description>True if operation succeeded</description></item>
        ///   <item><term>Message</term><description>Status message or error description</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Successful deposits are automatically persisted.
        /// </remarks>
        public (bool Success, string Message) Deposit(BankAccount? account, decimal amount)
        {
            if (account == null)
                return (false, "Account cannot be null.");
            if (amount <= 0)
                return (false, "Deposit amount must be greater than zero.");

            try
            {
                account.Deposit(amount);
                _userService.Save();
                return (true, $"Successfully deposited {amount:C}.");
            }
            catch (Exception ex)
            {
                return (false, $"Deposit failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes a withdrawal transaction from the specified account.
        /// </summary>
        /// <param name="account">The source account. Must not be null.</param>
        /// <param name="amount">The positive amount to withdraw.</param>
        /// <returns>
        /// A tuple where:
        /// <list type="table">
        ///   <item><term>Success</term><description>True if operation succeeded</description></item>
        ///   <item><term>Message</term><description>Status message or error description</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Validates sufficient funds before processing.
        /// Successful withdrawals are automatically persisted.
        /// </remarks>
        public (bool Success, string Message) Withdraw(BankAccount? account, decimal amount)
        {
            if (account == null)
                return (false, "Account cannot be null.");
            if (amount <= 0)
                return (false, "Withdrawal amount must be greater than zero.");

            try
            {
                account.Withdraw(amount);
                _userService.Save();
                return (true, $"Successfully withdrew {amount:C}.");
            }
            catch (Exception ex)
            {
                return (false, $"Withdrawal failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes a transfer between two accounts.
        /// </summary>
        /// <param name="sender">The source account. Must not be null.</param>
        /// <param name="recipient">The destination account. Must not be null.</param>
        /// <param name="amount">The positive amount to transfer.</param>
        /// <returns>
        /// A tuple where:
        /// <list type="table">
        ///   <item><term>Success</term><description>True if operation succeeded</description></item>
        ///   <item><term>Message</term><description>Status message or error description</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Validates:
        /// <list type="bullet">
        ///   <item><description>Different accounts</description></item>
        ///   <item><description>Sufficient funds</description></item>
        ///   <item><description>Positive amount</description></item>
        /// </list>
        /// Successful transfers are automatically persisted.
        /// </remarks>
        public (bool Success, string Message) Transfer(BankAccount? sender, BankAccount? recipient, decimal amount)
        {
            if (sender == null || recipient == null)
                return (false, "Both sender and recipient accounts must be provided.");
            if (sender == recipient)
                return (false, "Cannot transfer to the same account.");
            if (amount <= 0)
                return (false, "Transfer amount must be greater than zero.");

            try
            {
                sender.Transfer(amount, recipient);
                _userService.Save();
                return (true, $"Successfully transferred {amount:C}.");
            }
            catch (Exception ex)
            {
                return (false, $"Transfer failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all accounts of a specific type for the given user.
        /// </summary>
        /// <param name="user">The account owner. Must not be null.</param>
        /// <param name="type">The account type to filter by.</param>
        /// <returns>A list of matching <see cref="BankAccount"/> objects.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
        public List<BankAccount> GetAccountsByType(User? user, AccountType type)
        {
            if (user == null) 
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
                
            return user.Accounts
                     .Where(account => account.Type == type)
                     .ToList();
        }
    }
}