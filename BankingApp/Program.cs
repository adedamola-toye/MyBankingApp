using BankingApp.Models;
using BankingApp.Interfaces;
using BankingApp.Services;
using BankingApp.Data;
using BankingApp.Helpers;


/// <summary>
/// Main entry point for the BankSim console application.
/// </summary>
/// <remarks>
/// This program provides a console-based interface for:
/// <list type="bullet">
///   <item><description>User registration and authentication</description></item>
///   <item><description>Bank account management</description></item>
///   <item><description>Financial transactions (deposits, withdrawals, transfers)</description></item>
///   <item><description>Transaction history viewing</description></item>
/// </list>
/// </remarks>
public class Program
{
    /// <summary>
    /// Main method
    /// </summary>
    public static void Main()
    {
        Console.Title = "BankSim";

        // Initialize service dependencies
        IDataStore<string, User> dataStore = new UserFileDataStore();
        IAuthService authService = new AuthService();
        IUserService userService = new UserService(dataStore, authService);
        IBankService bankService = new BankService(userService);
        ITransactionService transactionService = new TransactionService();

        User? loggedInUser = null;


        while (true)
        {
            if (loggedInUser == null)
            {
                // Display authentication menu
                Console.Clear();
                Console.WriteLine(@"
╔═══════════════════════════════════╗
║         🏦  Welcome to BankSim      ║
╚═══════════════════════════════════╝
");

                Console.WriteLine("Please select an option: ");
                Console.WriteLine("  [1] 🔐 Register");
                Console.WriteLine("  [2] 🔑 Log In");
                Console.WriteLine("  [3] 🚪 Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("\n════════════ 🔐 User Registration ════════════");
                        Console.WriteLine("Enter Username (or 'C' to cancel): ");
                        var username = Console.ReadLine();
                        if (UserInteraction.ShouldCancel(username))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }



                        Console.WriteLine("Enter Password (or 'C' to cancel): ");
                        var password = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(password))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                        {
                            Console.WriteLine("❌ Username and password is required ");
                        }
                        else
                        {
                            var registerResult = userService.Register(username, password);
                            Console.WriteLine(registerResult.Message);
                        }
                        break;

                    case "2":
                        Console.WriteLine("\n════════════ 🔑 User Login ════════════");
                        Console.WriteLine("Enter Username (or 'C' to cancel): ");
                        username = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(username))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        Console.WriteLine("Enter Password (or 'C' to cancel): ");
                        password = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(password))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                        {
                            Console.WriteLine("❌ Username and password is required");
                        }
                        else
                        {
                            var loginResult = userService.Login(username, password);
                            Console.WriteLine(loginResult.Message);
                            if (loginResult.Success)
                            {
                                loggedInUser = loginResult.User;
                            }
                        }
                        break;

                    case "3":
                        Console.WriteLine("\n════════════ 🚪 Exiting.......Goodbye👋.════════════");
                        return;

                    default:
                        Console.WriteLine("❌ Invalid input. Try again.");
                        break;
                }
            }
            else
            {
                // Display banking menu for authenticated users
                Console.WriteLine($"\nWelcome, {loggedInUser.Username}🎉");
                Console.WriteLine("\n════════════ 💼 Banking Menu ════════════");

                Console.WriteLine("  [1] 🏦 Create Bank Account");
                Console.WriteLine("  [2] 💵 Deposit");
                Console.WriteLine("  [3] 💸 Withdraw");
                Console.WriteLine("  [4] 🔁 Transfer");
                Console.WriteLine("  [5] 📊 View Balance");
                Console.WriteLine("  [6] 📜 View Transactions");
                Console.WriteLine("  [7] 🚪 Log Out");
                Console.WriteLine("══════════════════════════════════════════════════");

                Console.Write("Select an option: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": // Account creation logic
                        Console.WriteLine("\n════════════ 🏦 Create Bank Account ═══════════");
                        Console.WriteLine("Choose account type (1. Savings, 2. Current) or 'C' to cancel:");
                        var acctType = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(acctType))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (!new[] { "1", "2" }.Contains(acctType))
                        {
                            Console.WriteLine("❌ Invalid selection");
                            break;
                        }
                        BankAccount? newAcct = null;

                        if (acctType == "1")
                        {
                            newAcct = bankService.CreateAccount(loggedInUser, AccountType.Savings);
                        }
                        else if (acctType == "2")
                        {
                            newAcct = bankService.CreateAccount(loggedInUser, AccountType.Current);
                        }
                        else
                        {
                            Console.WriteLine("❌ Invalid input. Enter only input 1 and 2");
                            break;
                        }

                        Console.WriteLine($"✅ Account created successfully. \nAccount Number: {newAcct.AccountNo}");
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "2": // Deposit logic
                        Console.WriteLine("\n════════════ 💵 Deposit Funds ═══════════");
                        if (loggedInUser.Accounts.Count == 0)
                        {
                            Console.WriteLine("❌ You don't have any bank accounts. Please create one first.");
                            Console.WriteLine("Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }

                        var depositAccount = AccountSelector.SelectAccount(loggedInUser);
                        Console.Write("\n💵 Amount to deposit (or 'C' to cancel): ");
                        var depositInput = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(depositInput))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (!decimal.TryParse(depositInput, out var depositAmount) || depositAmount <= 0)
                        {
                            Console.WriteLine("❌ Invalid amount");
                            break;
                        }

                        if (!UserInteraction.ConfirmAction($"Deposit ₦{depositAmount:N2} to {depositAccount.AccountNo}"))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (depositAmount <= 0)
                        {
                            Console.WriteLine("❌Amount must be greater than zero. ");
                            break;
                        }

                        var depositResult = bankService.Deposit(depositAccount, depositAmount);
                        Console.WriteLine($"✅{depositResult.Message}");
                        Console.WriteLine($"💼New Balance: ₦{depositAccount.Balance:N2}");

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "3": // Withdrawal logic
                        Console.WriteLine("\n════════════ 💸 Withdraw Funds ═══════════");
                        if (loggedInUser.Accounts.Count == 0)
                        {
                            Console.WriteLine("❌You don't have any bank accounts. Please create one first.");
                            Console.WriteLine("Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }

                        var withdrawAcct = AccountSelector.SelectAccount(loggedInUser);
                        Console.Write("\n💵 Amount to withdraw (or 'C' to cancel): ");
                        var withdrawInput = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(withdrawInput))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (!decimal.TryParse(withdrawInput, out var withdrawAmount) || withdrawAmount <= 0)
                        {
                            Console.WriteLine("❌ Invalid amount");
                            break;
                        }

                        if (!UserInteraction.ConfirmAction($"Withdraw ₦{withdrawAmount:N2} from {withdrawAcct.AccountNo}"))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (withdrawAmount <= 0)
                        {
                            Console.WriteLine("❌Amount must be greater than zero. ");
                            break;
                        }

                        var withdrawResult = bankService.Withdraw(withdrawAcct, withdrawAmount);
                        Console.WriteLine($"{withdrawResult.Message}");
                        Console.WriteLine($"💼New Balance: ₦{withdrawAcct.Balance:N2}");

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "4": // Transfer logic
                        Console.WriteLine("\n════════════ 🔁 Transfer Funds ═══════════");
                        if (loggedInUser.Accounts.Count == 0)
                        {
                            Console.WriteLine("\n❌You don't have any bank accounts. Please create one first.");
                            Console.WriteLine("\n Press any key to return to menu...");
                            Console.ReadKey();
                            break;
                        }

                        var senderAcct = AccountSelector.SelectAccount(loggedInUser);
                        Console.Write("\n👤 Recipient account number (or 'C' to cancel): ");
                        var recipientAcctNo = Console.ReadLine();

                        if (UserInteraction.ShouldCancel(recipientAcctNo!))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(recipientAcctNo))
                        {
                            Console.WriteLine("❌Recipient account number is required.");
                            break;
                        }
                        var recipientResult = AccountFinder.FindAcctByAcctNo(userService, recipientAcctNo);
                        if (!recipientResult.Success || recipientResult.Account == null)
                        {
                            Console.WriteLine(recipientResult.Message);
                            break;
                        }



                        if (senderAcct.AccountNo == recipientAcctNo)
                        {
                            Console.WriteLine("❌ Cannot transfer to the same account.");
                            break;
                        }

                        var result = AccountFinder.FindAcctByAcctNo(userService, recipientAcctNo);
                        if (!result.Success)
                        {
                            Console.WriteLine(result.Message);
                            break;
                        }

                        var recipientAcct = result.Account!;
                        Console.Write("\n💵 Amount to transfer (or 'C' to cancel): ");
                        var transferAmountInput = Console.ReadLine();
                        var isValidAmount = decimal.TryParse(transferAmountInput, out var transferAmount);

                        if (UserInteraction.ShouldCancel(transferAmountInput))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (!UserInteraction.ConfirmAction($"Transfer ₦{transferAmount:N2} to {recipientResult.Account.AccountNo}"))
                        {
                            UserInteraction.ShowCancelledMessage();
                            break;
                        }

                        if (!isValidAmount || transferAmount <= 0)
                        {
                            Console.WriteLine("❌Invalid amount.");
                            break;
                        }

                        try
                        {
                            var transferResult = bankService.Transfer(senderAcct, recipientAcct, transferAmount);
                            Console.WriteLine($"✅{transferResult.Message}");
                            Console.WriteLine($"💼New Balance: ₦{senderAcct.Balance:N2}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Transfer failed: {ex.Message}");
                        }

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "5": // View balance
                        var selectedAcct = AccountSelector.SelectAccount(loggedInUser);
                        Console.WriteLine("\n═════════ 📄 Account Summary ═════════");
                        Console.WriteLine($"👤 Account Holder: {loggedInUser.Username}");
                        Console.WriteLine($"🏦 Account No:     {selectedAcct.AccountNo}");
                        Console.WriteLine($"📂 Account Type:   {selectedAcct.Type}");
                        Console.WriteLine($"💰 Balance:        ₦{selectedAcct.Balance:N2}");
                        Console.WriteLine("══════════════════════════════════════");

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "6": // View transactions
                        Console.WriteLine("\n═════════ 📜 Getting Transaction History ═════════");
                        try
                        {
                            var transactionAcct = AccountSelector.SelectAccount(loggedInUser);
                            var transactionList = transactionService.GetTransactions(transactionAcct).OrderByDescending(transaction => transaction.Date);

                            if (!transactionList.Any())
                            {
                                Console.WriteLine("No transactions found for this account.");
                            }
                            else
                            {
                                Console.WriteLine("\n📜 Transaction History");
                                Console.WriteLine($"\nAccount: {transactionAcct.AccountNo}");
                                Console.WriteLine($"Current Balance: ₦{transactionAcct.Balance:N2}");
                                Console.WriteLine("══════════════════════════════════════════");
                                Console.WriteLine("══════════════════════════════════════════");
                                foreach (var transaction in transactionList)
                                {
                                    Console.WriteLine($"🕒 {transaction.Date:dd/MM/yyyy HH:mm:ss.fff}");
                                    Console.WriteLine($"🔢 Transaction ID: {transaction.Id}");
                                    Console.WriteLine($"🔁 Type:        {transaction.Type}");
                                    Console.WriteLine($"💳 Amount:      ₦{transaction.Amount:N2}");
                                    Console.WriteLine($"💰 Before:      ₦{transaction.BalanceBeforeTransaction:N2}");
                                    Console.WriteLine($"💰 After:       ₦{transaction.BalanceAfterTransaction:N2}");

                                    Console.WriteLine($"📄 Description: {transaction.Description}");

                                    if (transaction.Type == TransactionType.Transfer && transaction.OtherPartyAcct != null)
                                    {
                                        Console.WriteLine($"👥 Other Party Account: {transaction.OtherPartyAcct.AccountNo}");
                                    }
                                    Console.WriteLine("----------------------------------------");
                                }
                                Console.WriteLine("──────────────────────────────");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            UserInteraction.ShowCancelledMessage();
                            continue;
                        }

                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;

                    case "7": // Log out
                        Console.WriteLine("🚪Logging out...");
                        loggedInUser = null;
                        break;

                    default:
                        Console.WriteLine("❌ Invalid option. Please try again.");
                        break;
                }
            }
        }
    }

}
