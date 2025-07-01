Program Structure

BankSim/
├── Models/           → Data classes like User, Account, Transaction
├── Interfaces/       → Definitions for what each service  or model must do
├── Services/         → The brains (register user, deposit money, etc.)
├── Data/             → Reading/writing data to file, managing memory
├── Helpers/            → Hashing passwords, validations
└── Program.cs        → The menu-based app flow
