using System.Collections.Generic;


namespace BankingApp.Models
{
    public class User
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be empty.");
                }
                _username = value;
            }
        }

        public string PasswordHash { get; set; }
        public List<BankAccount> Accounts { get; set; }

        public User(string username, string passwordHash)
        {
            this.Username = username;
            this.PasswordHash = passwordHash;
            this.Accounts = new List<BankAccount>(); //create an empty list object for the list of accounts
        }
    }
}