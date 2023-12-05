using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.ValueObjects;

[Owned]
public record Attendee(Guid AccountId, DateTimeOffset AcceptedDate)
{
}
