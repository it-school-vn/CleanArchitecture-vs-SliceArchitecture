namespace CleanArchitecture.Infrastructure.Auth.Github
{
    public class GithubValidationTokenException : Exception
    {
        public GithubValidationTokenException(string message, Exception? innerException)
        : base($"Github validation token exception {message}", innerException)
        {
        }
    }
}