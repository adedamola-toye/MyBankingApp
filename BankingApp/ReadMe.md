# 🏦 BankSim - Secure Banking System

A .NET 9 console application for managing bank accounts and transactions with full documentation support.

## ✨ Key Features

| Feature | Description |
|---------|-------------|
| 🔐 **Secure Authentication** | User registration and login with password hashing |
| 💼 **Account Management** | Create and manage savings/current accounts |
| 💰 **Transaction Processing** | Deposit, withdraw, and transfer funds |
| 📜 **Transaction History** | View complete audit trails |
| 📝 **Self-Documenting** | Full XML comment support |

## 🚀 Quick Start

### Prerequisites
- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/) (optional)

### Installation
```bash
git clone https://github.com/your-username/BankingApp.git
cd BankingApp


```

### Running the Application
```bash
```dotnet run

```

## 📂 Project Structure
```bash
BankSim/
├── Models/           # Data models and entities
│   ├── User.cs       # User profiles and credentials
│   ├── Account.cs    # Base account functionality
│   └── Transaction.cs # Financial transaction records
├── Interfaces/       # Service contracts
├── Services/         # Core business logic
│   ├── AuthService.cs # Authentication
│   └── BankService.cs # Account operations
├── Data/             # Data persistence
├── Helpers/          # Utilities
└── Program.cs        # Application entry point

``` 

## Testing
```bash
dotnet test

```
## Generate docs
```bash
./generate-docs.ps1  (in windows)