namespace CleanArchitecture.Application.Core.CustomExceptions
{
    public sealed class DependencyException : Exception
    {
        public DependencyException(string serviceName)
        : base($"Cannot get the service {serviceName}")
        {
        }
    }
}