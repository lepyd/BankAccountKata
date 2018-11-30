using System;

namespace ClientAccount.Domain.Model
{
    public class AppliedOperation
    {
        public AppliedOperation(IOperation operation, decimal balanceBeforeApply, decimal balanceAferApply, DateTime appliedDate)
        {
            Operation = operation;
            BalanceBeforeApply = balanceBeforeApply;
            BalanceAfterApply = balanceAferApply;
            AppliedDate = appliedDate;
        }

        public IOperation Operation { get; }
        public decimal BalanceBeforeApply { get; }
        public decimal BalanceAfterApply { get; }
        public DateTime AppliedDate { get; }

        public override string ToString()
        {
            return $"{AppliedDate}:{BalanceBeforeApply}=>{BalanceAfterApply}: {Operation}";
        }
    }
}
