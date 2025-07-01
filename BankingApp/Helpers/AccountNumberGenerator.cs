namespace BankingApp.Helpers
{
    public static class AccountNumberGenerator
    {
        public static string Generate()
        {
            return DateTime.Now.Ticks.ToString.Substring(0, 10);
        }
    }
}