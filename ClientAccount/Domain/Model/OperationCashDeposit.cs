using System;

namespace ClientAccount.Domain.Model
{
    public class OperationCashDeposit : OperationBase, IOperation
    {
        public OperationCashDeposit(AccountId accountId, decimal amount) : base(accountId, amount)
        {
        }

        public OperationResult ApplyOn(Account account)
        {
            if (!IsValidFor(account))
                return OperationResult.AsRejected("rejected");

            var appliedOperation = new AppliedOperation(
                    this,
                    account.CurrentBalance,
                    account.CurrentBalance + Amount,
                    DateTime.Now
                );

            return OperationResult.AsDone(appliedOperation);
        }

        public bool IsValidFor(Account account)
        {
            return Amount <= 10000;
        }

        public override string ToString()
        {
            return $"CashDeposit {Amount}";
        }
    }
}
