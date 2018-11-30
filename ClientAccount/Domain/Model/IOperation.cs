namespace ClientAccount.Domain.Model
{
    public interface IOperation
    {
        AccountId AccountId { get; }
        decimal Amount { get; }

        bool IsValidFor(Account account);
        OperationResult ApplyOn(Account account);
    }
}
