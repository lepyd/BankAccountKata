using System;

namespace ClientAccount.Domain.Model
{
    public class CustomerId : IEquatable<CustomerId>
    {
        public CustomerId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public bool Equals(CustomerId other)
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
