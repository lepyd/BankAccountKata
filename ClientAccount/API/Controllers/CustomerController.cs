using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ClientAccount.API.Dto;
using ClientAccount.Domain.Model;
using ClientAccount.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ClientAccount.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerService _customerService;
        IAccountService _accountService;

        public CustomerController(
            ICustomerService customerService,
            IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }

        [HttpGet("{firstName}/{lastName}")]
        [ProducesResponseType(200, Type = typeof(CustomerIdDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public IActionResult CreateCustomer([Required] string firstName, [Required] string lastName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var initialCustomerInfo = new InitialCustomerInfo
            {
                FirstName = firstName,
                LastName = lastName
            };

            try
            {
                var customerId = _customerService.CreateCustomerWithOneAccount(initialCustomerInfo);

                var dto = new CustomerIdDto { Id = customerId.Id };

                return Ok(dto);

            }
            catch (Exception e)
            {
                return Conflict(e);
            }
        }

        // POST api/values
        [HttpGet("{firstName}/{lastName}")]
        [ProducesResponseType(200, Type = typeof(CustomerIdDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult GetCustomerId([Required] string firstName, [Required] string lastName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerId = _customerService.GetCustomerIdFromName(firstName, lastName);

            if(customerId == null)
            {
                return NoContent();
            }

            var dto = new CustomerIdDto { Id = customerId.Id };

            return Ok(dto);
        }

        [HttpPut("{customerId}/{amount}")]
        [ProducesResponseType(200, Type = typeof(OperationDto))]
        [ProducesResponseType(400)]
        public IActionResult ApplyCashWithdrawal([Required] CustomerIdDto customerId, [Required] decimal amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cid = new CustomerId(customerId.Id);

            var customerInfo = _customerService.GetCustomerInfoFromId(cid);

            var operation = new OperationCashWithdrawal(customerInfo.MasterAccountId, amount);

            var operationResult = _accountService.ApplyOperation(operation);

            var dto = Map(operationResult);

            return Ok(dto);
        }

        private OperationDto Map(OperationResult operationResult)
        {
            OperationDto dto;
            if (operationResult.Status == OperationStatus.Done)
            {
                dto = new OperationDto
                {
                    Status = operationResult.Status.ToString(),
                    Amount = operationResult.Result.Operation.Amount,
                    BalanceAfterApply = operationResult.Result.BalanceAfterApply,
                    AppliedDate = operationResult.Result.AppliedDate,
                    Description = operationResult.Result.ToString()
                };
            }
            else
            {
                dto = new OperationDto
                {
                    Status = operationResult.Status.ToString(),
                    Description = operationResult.Comment
                };
            }
            return dto;
        }

        [HttpPut("{customerId}/{amount}")]
        [ProducesResponseType(200, Type = typeof(OperationDto))]
        [ProducesResponseType(400)]
        public IActionResult ApplyCashDeposit([Required] CustomerIdDto customerId, [Required] decimal amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cid = new CustomerId(customerId.Id);

            var customerInfo = _customerService.GetCustomerInfoFromId(cid);

            var operation = new OperationCashDeposit(customerInfo.MasterAccountId, amount);

            var operationResult = _accountService.ApplyOperation(operation);

            var dto = Map(operationResult);

            return Ok(dto);
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(List<string>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult OperationHistory([Required] CustomerIdDto customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cid = new CustomerId(customerId.Id);

            var customerInfo = _customerService.GetCustomerInfoFromId(cid);

            if (customerInfo == null) return NoContent();

            var account = _accountService.GetAccountFromId(customerInfo.MasterAccountId);

            var history = account.OperationHistory.Select(op => op.ToString()).ToList();

            return Ok(history);
        }
    }
}
