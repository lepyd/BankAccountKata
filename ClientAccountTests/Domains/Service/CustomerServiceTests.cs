using NUnit.Framework;
using ClientAccount.Domain.Service.Implementation;
using ClientAccount.Domain.Service;
using ClientAccount.Domain.Model;
using System;

namespace ClientAccountTests.Domains.Service
{
    class CustomerServiceTests
    {
        IStoreService _store;
        IAccountService _accountService;

        [SetUp]
        public void Setup()
        {
            _store = new StoreService(); // no need to use moq due to the current implemntation
            _accountService = new AccountService(_store);
        }

        [Test]
        public void CreateCustomer_WithValidInfo_Succeed()
        {
            var customerService = new CustomerService(_store, _accountService);

            var initialCustomerInfo = new InitialCustomerInfo
            {
                FirstName = "FirstName1",
                LastName = "lastName"
            };

            // Creating the customer
            var customerId = customerService.CreateCustomerWithOneAccount(initialCustomerInfo);

            Assert.NotNull(customerId);
            Assert.NotNull(customerId.Id);
            Assert.AreNotEqual(Guid.Empty, customerId);

            // Info is available for the customer
            var customerInfo = customerService.GetCustomerInfoFromId(customerId);

            Assert.NotNull(customerInfo);
            Assert.AreEqual(customerId, customerInfo.CustomerId);
            Assert.AreEqual(initialCustomerInfo.FirstName, customerInfo.FirstName);
            Assert.AreEqual(initialCustomerInfo.LastName, customerInfo.LastName);
            Assert.NotNull(customerInfo.MasterAccountId);
            Assert.AreNotEqual(Guid.Empty, customerInfo.MasterAccountId);

            // The customer has an account
            var account = _accountService.GetAccountFromId(customerInfo.MasterAccountId);

            Assert.NotNull(account);
            Assert.AreEqual(0, account.CurrentBalance);
            Assert.AreEqual(0, account.InitialBalance);
            Assert.IsEmpty(account.OperationHistory);
        }
    }
}
