using System.Text.Json.Serialization;

namespace CleanArchitecture.Application.Core.Abstraction.Message
{
    public interface IOwnerCommand<TId, out TResponse> : ICommand<TResponse>
    {
        [JsonIgnore]
        public TId Id { get; set; }
    }
}