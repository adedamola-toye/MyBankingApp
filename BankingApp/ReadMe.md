# 🏦 BankSim

A secure .NET 9 banking system with account management and transaction processing.

## Key Features
- ✅ User authentication (Register/Login)  
- 💳 Account management (Create/View accounts)  
- 💸 Transactions (Deposit/Withdraw/Transfer)  
- 📊 Transaction history 

## Quick Start
```bash
dotnet run

#### 2. **`docs/index.md` (Detailed Documentation)**
```markdown
# 📚 BankingApp Documentation



Program Structure

BankSim/
├── Models/           → Data classes like User, Account, Transaction
├── Interfaces/       → Definitions for what each service  or model must do
├── Services/         → The brains (register user, deposit money, etc.)
├── Data/             → Reading/writing data to file, managing memory
├── Helpers/            → Hashing passwords, validations
└── Program.cs        → The menu-based app flow
