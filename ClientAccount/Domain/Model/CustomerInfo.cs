namespace ClientAccount.Domain.Model
{
    public class CustomerInfo
    {
        public CustomerId CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AccountId MasterAccountId { get; set; } 
    }
}
