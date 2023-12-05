using System.Collections.ObjectModel;

namespace CleanArchitecture.Application.Core.CustomExceptions;

public class BadRequestException : CustomApplicationException
{
    public ReadOnlyDictionary<string, string[]> Errors { get; set; }
    public BadRequestException(ReadOnlyDictionary<string, string[]> error) :
    base("Bad request", $"Bad request.")
    {
        Errors = error;
    }


    public static BadRequestException BadRequest(string message) => BadRequest("UnKnown", message);
    public static BadRequestException BadRequest(string fieldName, string message) => BadRequest(new Dictionary<string, string[]> { { fieldName, new[] { message } } });

    public static BadRequestException BadRequest(Dictionary<string, string[]> errors) => new(new ReadOnlyDictionary<string, string[]>(errors));



}