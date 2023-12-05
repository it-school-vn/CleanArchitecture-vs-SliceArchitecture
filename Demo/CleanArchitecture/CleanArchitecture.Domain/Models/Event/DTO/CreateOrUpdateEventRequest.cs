using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Event.Enum;
using CleanArchitecture.Domain.Models.Event.ValueObjects;
using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Domain.Models.Event.DTO;
public class CreateOrUpdateEventRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? BannerUrl { get; set; }

    public required decimal Fee { get; set; }

    public EventType EventType { get; set; }

    public ConferenceOption? Conference { get; set; }

    public Address? Location { get; set; }

    public int MaxAttendee { get; set; } = 1;

    public DateOnly OpenDate { get; set; }

    public TimeOnly OpenTime { get; set; }

    public DateOnly ClosedDate { get; set; }

    public TimeOnly ClosedTime { get; set; }

    public DateOnly DateAt { get; set; }

    public TimeOnly TimeAt { get; set; }

    public int Duration { get; set; }

    public string[]? Tags { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;
}