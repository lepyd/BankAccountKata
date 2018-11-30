namespace ClientAccount.Domain.Model
{
    public class OperationBase
    {
        public OperationBase(AccountId accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }

        public AccountId AccountId { get; }

        public decimal Amount { get; }
    }
}
