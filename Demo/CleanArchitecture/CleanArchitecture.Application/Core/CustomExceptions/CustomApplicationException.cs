namespace CleanArchitecture.Application.Core.CustomExceptions
{
    public class CustomApplicationException : Exception
    {
        public string Title { get; set; }

        public CustomApplicationException(string title, string message) : base(message)
        {
            Title = title;
        }

        protected CustomApplicationException(string title, string message, Exception? innerException) : base(message, innerException)
        {
            Title = title;
        }

    }
}