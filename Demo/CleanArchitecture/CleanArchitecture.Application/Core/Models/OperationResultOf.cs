#nullable disable
using System.Net;

namespace CleanArchitecture.Application.Core.Models
{
    public sealed class OperationResult<T> : BaseOperationResult
    {
        public OperationResult(HttpStatusCode statusCode, T value)
        {
            StatusCode = statusCode;
            Value = value;
        }

        public OperationResult(OperationResult result, T value = default)
        {
            StatusCode = result.StatusCode;
            Value = value;
        }

        public T Value { get; set; }

        public OperationResult ToOperationResult()
        {
            return new OperationResult(StatusCode);
        }

        public static implicit operator OperationResult<T>(OperationResult result)
        {
            return new OperationResult<T>(result);
        }
    }
}