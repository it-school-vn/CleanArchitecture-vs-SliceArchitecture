namespace CleanArchitecture.Infrastructure.Auth.Google
{
    public class GoogleValidationTokenException : Exception
    {
        public GoogleValidationTokenException(string message, Exception? innerException)
        : base($"Google validation token exception {message}", innerException)
        {
        }
    }
}