using System;
using System.Collections.Generic;
using System.Linq;
using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service.Implementation
{
    public class StoreService : IStoreService
    {
        private Dictionary<AccountId, Account> _accounts = new Dictionary<AccountId, Account>();
        private Dictionary<CustomerId, CustomerInfo> _customers = new Dictionary<CustomerId, CustomerInfo>();

        public Account CreateAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (account.AccountId == null) throw new ArgumentNullException(nameof(account.AccountId));

            if(_accounts.TryGetValue( account.AccountId, out var existingAccount))
            {
                throw new ArgumentException($"AccountId={account.AccountId} already exists");
            }
            _accounts.Add(account.AccountId, account);
            return account;
        }

        public CustomerInfo CreateCustomer(CustomerInfo customerInfo)
        {
            if (customerInfo == null) throw new ArgumentNullException(nameof(customerInfo));
            if (customerInfo.CustomerId == null) throw new ArgumentNullException(nameof(customerInfo.CustomerId));

            if (_customers.TryGetValue(customerInfo.CustomerId, out var existingAccount))
            {
                throw new ArgumentException($"CustomerId={customerInfo.CustomerId} already exists");
            }
            _customers.Add(customerInfo.CustomerId, customerInfo);
            return customerInfo;
        }

        public Account GetAccountFromId(AccountId accountId)
        {
            if (accountId == null) throw new ArgumentNullException(nameof(accountId));
            if (_accounts.TryGetValue(accountId, out var existingAccount))
                return existingAccount;
            return null;
        }

        public CustomerId GetCustomerByName(string firstName, string lastName)
        {
            var customerInfo = _customers.Values
                .FirstOrDefault(customer => customer.FirstName == firstName && customer.LastName == lastName);

            return customerInfo == null ? null : customerInfo.CustomerId;
        }

        public CustomerInfo GetCutomerInfoFromId(CustomerId customerId)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            if (_customers.TryGetValue(customerId, out var existingCustomer))
                return existingCustomer;
            return null;
        }

        public AppliedOperation SaveOperation(AppliedOperation appliedOperation)
        {
            var account = GetAccountFromId(appliedOperation.Operation.AccountId);

            // TODO check account.CurrentBalance == appliedOperation.BalanceBeforeApply
            account.CurrentBalance = appliedOperation.BalanceAfterApply;
            account.OperationHistory.Add(appliedOperation);

            return appliedOperation;
        }
    }
}
