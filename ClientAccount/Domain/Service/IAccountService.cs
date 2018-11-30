using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service
{
    public interface IAccountService
    {
        Account CreateAccount(decimal initialBalance);

        Account GetAccountFromId(AccountId accountId);

        OperationResult ApplyOperation(IOperation operation);

    }
}
