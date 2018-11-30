using System;

namespace ClientAccount.Domain.Model
{
    public class OperationCashWithdrawal : OperationBase, IOperation
    {
        public OperationCashWithdrawal(AccountId accountId, decimal amount) : base(accountId, amount)
        {
        }

        public OperationResult ApplyOn(Account account)
        {
            if (!IsValidFor(account))
                return OperationResult.AsRejected("rejected");

            var appliedOperation = new AppliedOperation(
                    this,
                    account.CurrentBalance,
                    account.CurrentBalance - Amount,
                    DateTime.Now
                );

            return OperationResult.AsDone(appliedOperation);
        }

        public bool IsValidFor(Account account)
        {
            return account.CurrentBalance - Amount > 0;
        }

        public override string ToString()
        {
            return $"CashWithdrawal {Amount}";
        }
    }
}
