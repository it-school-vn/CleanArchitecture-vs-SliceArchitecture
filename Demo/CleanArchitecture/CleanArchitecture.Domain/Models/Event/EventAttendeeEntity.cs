using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Models.Event;
public sealed class EventAttendeeEntity : BaseTimestampEntity<Ulid>
{
    [Required]
    public Ulid EventId { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    public AccountEntity? Account { get; set; }

    public EventEntity? Event { get; set; }

    [Required]
    public decimal Fee { get; set; }

    [Required]
    public bool Paid { get; set; }

    [Required]
    public bool Confirmed { get; set; }

    public Ulid? TransactionId { get; set; }
}