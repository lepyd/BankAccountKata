using System;
using System.Collections.Generic;
using ClientAccount.Domain.Model;

namespace ClientAccount.Domain.Service.Implementation
{
    public class AccountService : IAccountService
    {
        IStoreService _store;

        public AccountService(IStoreService store)
        {
            _store = store;
        }

        public OperationResult ApplyOperation(IOperation operation)
        {
            try
            {
                var account = GetAccountFromId(operation.AccountId);
                var operationResult = operation.ApplyOn(account);

                if (operationResult.Status == OperationStatus.Done)
                {
                    _store.SaveOperation(operationResult.Result);
                }

                return operationResult;
            }
            catch (Exception e)
            {
                return OperationResult.AsFailed(e.Message);
            }
        }

        public Account CreateAccount(decimal initialBalance = 0)
        {
            var account = Account.AsNewAccount(
                new AccountId(Guid.NewGuid()), 
                initialBalance);

            var savedAccount = _store.CreateAccount(account);

            return savedAccount;
        }

        public Account GetAccountFromId(AccountId accountId)
        {
            return _store.GetAccountFromId(accountId);
        }
    }
}
