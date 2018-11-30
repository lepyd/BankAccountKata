using System.Collections.Generic;

namespace ClientAccount.Domain.Model
{
    public class Account
    {
        private Account(AccountId accountId, decimal initialBalance)
        {
            AccountId = accountId;
            InitialBalance = initialBalance;
            CurrentBalance = initialBalance;
            OperationHistory = new List<AppliedOperation>();
        }

        public AccountId AccountId { get; }
        public decimal InitialBalance { get; }
        public decimal CurrentBalance { get; set; }
        public List<AppliedOperation> OperationHistory { get; }

        public static Account AsNewAccount(AccountId accountId, decimal initialBalance)
        {
            return new Account(accountId, initialBalance);
        }
    }
}
