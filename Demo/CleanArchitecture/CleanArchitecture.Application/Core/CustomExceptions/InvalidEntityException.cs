
namespace CleanArchitecture.Application.Core.CustomExceptions;

public class InvalidEntityException : CustomApplicationException
{
    public InvalidEntityException(string entityName) : base("Invalid Entity Exception", $"The object {entityName} is invalid.")
    {
    }
}
