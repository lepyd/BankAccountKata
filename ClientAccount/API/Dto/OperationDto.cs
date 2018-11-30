using System;

namespace ClientAccount.API.Dto
{
    public class OperationDto
    {
        public string Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public decimal BalanceAfterApply { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
