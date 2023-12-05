using Refit;

namespace CleanArchitecture.Infrastructure.Email.Brevo;

public interface IBreroEmailApi
{
    [Post("/smtp/email")]
    [Headers("accept: application/json", "content-type: application/json")]
    Task<HttpResponseMessage> SendAsync([Header("api-key")] string apiKey, [Body(BodySerializationMethod.Serialized)] BrevoEmail brevoEmail);
}