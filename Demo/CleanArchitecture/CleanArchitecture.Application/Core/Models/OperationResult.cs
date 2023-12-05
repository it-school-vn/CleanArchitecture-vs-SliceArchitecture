
using System.Net;

namespace CleanArchitecture.Application.Core.Models
{
    public sealed class OperationResult : BaseOperationResult
    {

        public OperationResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public static OperationResult Ok() => new OperationResult(HttpStatusCode.OK);
        public static OperationResult Accepted() => new OperationResult(HttpStatusCode.Accepted);

        public static OperationResult Status(HttpStatusCode statusCode) => new OperationResult(statusCode);

        public static OperationResult<T> Ok<T>(T value) => new OperationResult<T>(HttpStatusCode.OK, value);
        public static OperationResult<T> Create<T>(T value) => new OperationResult<T>(HttpStatusCode.Created, value);
        public static OperationResult<T> Accepted<T>(T value) => new OperationResult<T>(HttpStatusCode.Accepted, value);

    }
}