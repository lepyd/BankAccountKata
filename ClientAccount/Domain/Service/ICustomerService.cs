using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service
{
    public interface ICustomerService
    {
        CustomerId GetCustomerIdFromName(string firstName, string lastName);
        CustomerId CreateCustomerWithOneAccount(InitialCustomerInfo customerInfo);
        CustomerInfo GetCustomerInfoFromId(CustomerId customerId);
    }
}
