using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service
{
    public interface IStoreService
    {
        CustomerId GetCustomerByName(string firstName, string lastName);
        CustomerInfo CreateCustomer(CustomerInfo customerInfo);
        Account CreateAccount(Account account);
        Account GetAccountFromId(AccountId accountId);
        AppliedOperation SaveOperation(AppliedOperation appliedOperation);
        CustomerInfo GetCutomerInfoFromId(CustomerId customerId);
    }
}
