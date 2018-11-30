using System;
using System.Collections.Generic;
using NUnit.Framework;
using ClientAccount.Domain.Service.Implementation;
using ClientAccount.Domain.Service;
using ClientAccount.Controllers;
using Microsoft.AspNetCore.Mvc;
using ClientAccount.API.Dto;

namespace ClientAccountTests.Controllers
{
    class CustomerControllerTests
    {
        IAccountService _accountService;
        ICustomerService _customerService;

        [SetUp]
        public void SetUp()
        {
            var store = new StoreService();
            _accountService = new AccountService(store);
            _customerService = new CustomerService(store, _accountService);
        }

        [Test]
        public void CreateCustomer_WithValidArgument_Succeed()
        {
            var customerController = new CustomerController(_customerService, _accountService);

            var result = customerController.CreateCustomer("firstName1", "lastName");

            Assert.True(result is OkObjectResult);

            var okResult = result as OkObjectResult;

            Assert.True(okResult.Value is CustomerIdDto);

            var customerId = okResult.Value as CustomerIdDto;

            Assert.NotNull(customerId.Id);
            Assert.AreNotEqual(Guid.Empty, customerId.Id);
        }

        [Test]
        public void CreateCustomer_WithDuplicatedArgument_Fails()
        {
            var customerController = new CustomerController(_customerService, _accountService);

            var result1 = customerController.CreateCustomer("firstName2", "lastName");

            Assert.True(result1 is OkObjectResult);

            var result2 = customerController.CreateCustomer("firstName2", "lastName");

            Assert.True(result2 is ConflictObjectResult);
        }


        [Test]
        public void FullSequenceTest()
        {
            var customerController = new CustomerController(_customerService, _accountService);

            // Creating the customer
            var result = customerController.CreateCustomer("FirstNameForFullSequence", "lastName");

            Assert.True(result is OkObjectResult);
            var okResult = result as OkObjectResult;
            Assert.True(okResult.Value is CustomerIdDto);

            // Get the customer from name
            result = customerController.GetCustomerId("FirstNameForFullSequence", "lastName");

            Assert.True(result is OkObjectResult);
            okResult = result as OkObjectResult;
            Assert.True(okResult.Value is CustomerIdDto);
            var customerId = okResult.Value as CustomerIdDto;

            // Apply Cash Deposit on Customer's account
            result = customerController.ApplyCashDeposit(customerId, 1000);

            Assert.True(result is OkObjectResult);
            okResult = result as OkObjectResult;
            Assert.True(okResult.Value is OperationDto);
            var operation = okResult.Value as OperationDto;
            Assert.AreEqual(1000, operation.Amount);
            Assert.AreEqual(1000, operation.BalanceAfterApply);
            Assert.AreEqual("Done", operation.Status);
            Assert.True(operation.Description.Contains("CashDeposit"));

            // Apply Cash Withdrawal on Customer's account
            result = customerController.ApplyCashWithdrawal(customerId, 200);

            Assert.True(result is OkObjectResult);
            okResult = result as OkObjectResult;
            Assert.True(okResult.Value is OperationDto);
            operation = okResult.Value as OperationDto;
            Assert.AreEqual(200, operation.Amount);
            Assert.AreEqual(800, operation.BalanceAfterApply);
            Assert.AreEqual("Done", operation.Status);
            Assert.True(operation.Description.Contains("CashWithdrawal"));

            // Get operation History
            result = customerController.OperationHistory(customerId);

            Assert.True(result is OkObjectResult);
            okResult = result as OkObjectResult;
            Assert.True(okResult.Value is List<string>);
            var histo = okResult.Value as List<string>;
            Assert.AreEqual(2, histo.Count);
            Assert.True(histo[0].Contains("CashDeposit"));
            Assert.True(histo[1].Contains("CashWithdrawal"));
        }
    }
}
