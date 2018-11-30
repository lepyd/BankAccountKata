using NUnit.Framework;
using System;
using ClientAccount.Domain.Service.Implementation;
using ClientAccount.Domain.Service;
using ClientAccount.Domain.Model;

namespace ClientAccountTests.Domains.Service
{
    class AccountServiceTests
    {
        IStoreService _store;

        [SetUp]
        public void Setup()
        {
            _store = new StoreService(); // no need to use moq due to the current implemntation
        }

        [Test]
        public void CreateAccount_WithValidAccount_Succeed()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(100);

            Assert.NotNull(account);
            Assert.NotNull(account.AccountId);
            Assert.AreNotEqual(Guid.Empty, account.AccountId);
            Assert.AreEqual(100, account.CurrentBalance);
            Assert.AreEqual(100, account.InitialBalance);
            CollectionAssert.IsEmpty(account.OperationHistory);
        }

        [Test]
        public void ApplyOperation_WithValidCashDeposit_Succeed()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(100);

            var result = accountService.ApplyOperation(
                new OperationCashDeposit(
                    account.AccountId,
                    1000));

            Assert.NotNull(result);
            Assert.AreEqual(OperationStatus.Done, result.Status);
            Assert.AreEqual(100, result.Result.BalanceBeforeApply);
            Assert.AreEqual(1100, result.Result.BalanceAfterApply);
            Assert.That(DateTime.Now - result.Result.AppliedDate < new TimeSpan(0, 0, 1));
            Assert.That(result.Result.Operation is OperationCashDeposit);
            CollectionAssert.IsNotEmpty(account.OperationHistory);
        }

        [Test]
        public void ApplyOperation_WithValidCashWithdrawal_Succeed()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(1000);

            var result = accountService.ApplyOperation(
                new OperationCashWithdrawal(
                    account.AccountId,
                    100));

            Assert.NotNull(result);
            Assert.AreEqual(OperationStatus.Done, result.Status);
            Assert.AreEqual(1000, result.Result.BalanceBeforeApply);
            Assert.AreEqual(900, result.Result.BalanceAfterApply);
            Assert.That(DateTime.Now - result.Result.AppliedDate < new TimeSpan(0, 0, 1));
            Assert.That(result.Result.Operation is OperationCashWithdrawal);
            CollectionAssert.IsNotEmpty(account.OperationHistory);
        }

        [Test]
        public void ApplyOperation_WithInvalidCashWithdrawal_IsRejected()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(100);

            var result = accountService.ApplyOperation(
                new OperationCashWithdrawal(
                    account.AccountId,
                    1000));

            Assert.NotNull(result);
            Assert.AreEqual(OperationStatus.Rejected, result.Status);
            Assert.AreEqual("rejected", result.Comment);
        }

        [Test]
        public void ApplyOperation_WithInvalidCashDeposit_IsRejected()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(100);

            var result = accountService.ApplyOperation(
                new OperationCashDeposit(
                    account.AccountId,
                    100000));

            Assert.NotNull(result);
            Assert.AreEqual(OperationStatus.Rejected, result.Status);
            Assert.AreEqual("rejected", result.Comment);
        }

        [Test]
        public void GetAccount_WithExistingAccount_Succeed()
        {
            AccountService accountService = new AccountService(_store);

            var account = accountService.CreateAccount(100);

            var foundAccount = accountService.GetAccountFromId(account.AccountId);

            Assert.AreEqual(account, foundAccount);
        }


        [Test]
        public void GetAccount_WithInexistingAccount_ReturnsNull()
        {
            AccountService accountService = new AccountService(_store);

            var foundAccount = accountService.GetAccountFromId(new AccountId(Guid.NewGuid()));

            Assert.Null(foundAccount);
        }

        [Test]
        public void GetAccount_WithNullAccount_Throws()
        {
            AccountService accountService = new AccountService(_store);

            Assert.Throws<ArgumentNullException>(() => accountService.GetAccountFromId(null));
        }
    }
}
