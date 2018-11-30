using System;

namespace ClientAccount.Domain.Model
{
    public class AccountId : IEquatable<AccountId>
    {
        public AccountId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        bool IEquatable<AccountId>.Equals(AccountId other)
        {
            return Guid.Equals(this.Id, other.Id);
        }

        public override bool Equals(object other)
        {
            return Equals(other as CustomerId);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
