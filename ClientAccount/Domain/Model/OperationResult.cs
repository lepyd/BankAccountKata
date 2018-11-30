using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAccount.Domain.Model
{
    public enum OperationStatus
    {
        Done,
        Rejected,
        Failed
    }

    public class OperationResult : IEquatable<OperationResult>
    {
        public OperationStatus Status { get; private set; }
        public string Comment { get; private set; }
        public AppliedOperation Result { get; private set; }

        public static OperationResult AsRejected(string comment)
        {
            return new OperationResult
            {
                Status = OperationStatus.Rejected,
                Comment = comment
            };
        }

        public static OperationResult AsFailed(string comment)
        {
            return new OperationResult
            {
                Status = OperationStatus.Failed,
                Comment = comment
            };
        }

        public static OperationResult AsDone(AppliedOperation appliedOperation,string comment = null)
        {
            return new OperationResult
            {
                Status = OperationStatus.Done,
                Result = appliedOperation,
                Comment = comment
            };
        }

        bool IEquatable<OperationResult>.Equals(OperationResult other)
        {
            if (other == null) return false;
            return this.Status.Equals(other.Status)
                && string.Equals(this.Comment, other.Comment)
                && AppliedOperation.Equals(this.Result, other.Result);
        }
    }
}
