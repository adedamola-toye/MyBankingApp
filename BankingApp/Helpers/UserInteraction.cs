using BankingApp.Models;
namespace BankingApp.Helpers
{
    public static class UserInteraction
    {
        public  static bool ConfirmAction(string action)
        {
            Console.Write($"\n{action} - Confirm? (Y/N): ");
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Y) return true;
                if (key.Key == ConsoleKey.N) return false;
                Console.Beep();
            }
        }

        public  static bool ShouldCancel(string? input)
        {
            return input?.Trim().ToUpper() == "C";
        }

        public static void ShowCancelledMessage()
        {
            Console.WriteLine("\n🚫 Operation cancelled");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}