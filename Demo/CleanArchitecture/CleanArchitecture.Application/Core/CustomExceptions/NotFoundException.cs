namespace CleanArchitecture.Application.Core.CustomExceptions;

public class NotFoundException : CustomApplicationException
{
    public NotFoundException(string message) : base("Not found", message)
    {
    }
}