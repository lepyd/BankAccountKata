using System;
using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service.Implementation
{
    public class CustomerService : ICustomerService
    {
        readonly IStoreService _store;
        readonly IAccountService _accountService;

        public CustomerService(IStoreService store, IAccountService accountService)
        {
            _store = store;
            _accountService = accountService;
        }

        public CustomerId CreateCustomerWithOneAccount(InitialCustomerInfo customerFiles)
        {
            var previouslyExistingId = GetCustomerIdFromName(customerFiles.FirstName, customerFiles.LastName);

            if (previouslyExistingId != null) throw new ArgumentException("Customer already exists");

            var customerId = new CustomerId(Guid.NewGuid());

            var account = _accountService.CreateAccount(0);

            var customerInfo = new CustomerInfo()
            {
                CustomerId = customerId,
                FirstName = customerFiles.FirstName,
                LastName = customerFiles.LastName,
                MasterAccountId = account.AccountId
            };

            _store.CreateCustomer(customerInfo);

            return customerId;
        }

        public CustomerId GetCustomerIdFromName(string firstName, string lastName)
        {
            return _store.GetCustomerByName(firstName, lastName);
        }

        public CustomerInfo GetCustomerInfoFromId(CustomerId customerId)
        {
            return _store.GetCutomerInfoFromId(customerId);
        }
    }
}
