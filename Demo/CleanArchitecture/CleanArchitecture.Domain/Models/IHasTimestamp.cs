namespace CleanArchitecture.Domain.Entities;

public interface IHasTimestamp
{
    DateTimeOffset? Added { get; set; }
    DateTimeOffset? Modified { get; set; }
}