using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Event.Enum;
using CleanArchitecture.Domain.Models.Event.ValueObjects;
using CleanArchitecture.Domain.ValueObjects.ComplexType;
namespace CleanArchitecture.Domain.Models.Event.DTO;

public record EventResponse
{
    public Ulid Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? BannerUrl { get; set; }

    public required decimal Fee { get; set; }

    public EventType EventType { get; set; }

    public ConferenceOption? Conference { get; set; }

    public Address? Location { get; set; }

    public DateOnly DateAt { get; set; }

    public TimeOnly TimeAt { get; set; }

    public int Duration { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public Guid OwnerId { get; set; }

    public string[]? Tags { get; set; }
}