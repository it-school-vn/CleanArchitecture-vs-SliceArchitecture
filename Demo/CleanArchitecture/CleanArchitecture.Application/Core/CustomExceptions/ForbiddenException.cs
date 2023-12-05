namespace CleanArchitecture.Application.Core.CustomExceptions
{
    public class ForbiddenException : CustomApplicationException
    {
        public ForbiddenException() : base("Forbidden", "You have no privilege to access this resource")
        {

        }
    }
}