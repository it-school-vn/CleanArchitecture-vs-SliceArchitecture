#nullable disable
using System.Net;
namespace CleanArchitecture.Application.Core.Models
{
    public abstract class BaseOperationResult
    {
        public bool Succeeded => (int)StatusCode >= 200 && (int)StatusCode < 299;
        public HttpStatusCode StatusCode { get; set; }

    }
}