using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Account;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models.Event.Enum;
using CleanArchitecture.Domain.Models.Event.ValueObjects;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.ValueObjects.ComplexType;

namespace CleanArchitecture.Domain.Models.Event;

public sealed class EventEntity : BaseTimestampEntity<Ulid>, IOwner, IApproval, IDeletedEntity
{
    [Required]
    [MaxLength(250)]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    [MaxLength(Global.UrlLength)]
    [Unicode(false)]
    public string? BannerUrl { get; set; }

    [Required]
    public required decimal Fee { get; set; }

    [Required]
    public required decimal FeeRate { get; set; } = 0;

    [Required]
    public required EventType EventType { get; set; }

    [Required]
    public required ConferenceOption Conference { get; set; }

    [Required]
    public Address? Location { get; set; }

    [Required]
    public int MaxAttendee { get; set; } = 1;

    [Required]
    public required DateOnly OpenDate { get; set; }

    [Required]
    public required TimeOnly OpenTime { get; set; }


    [Required]
    public DateOnly ClosedDate { get; set; }

    [Required]
    public TimeOnly ClosedTime { get; set; }


    [Required]
    public required DateOnly DateAt { get; set; }

    [Required]
    public required TimeOnly TimeAt { get; set; }

    [Required]
    public required int Duration { get; set; }

    [Required]
    public required Guid OwnerId { get; set; }

    [Required]
    public AccountEntity? Owner { get; set; }

    [Required]
    public Approval Approval { get; set; } = new Approval(true, string.Empty);

    public ICollection<WithName>? Tags { get; set; }

    public ICollection<EventAttendeeEntity>? Attendees { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

    [Required]
    public bool Deleted { get; set; } = false;

    [Required]
    public byte TimeZone { get; set; } = 7;
}