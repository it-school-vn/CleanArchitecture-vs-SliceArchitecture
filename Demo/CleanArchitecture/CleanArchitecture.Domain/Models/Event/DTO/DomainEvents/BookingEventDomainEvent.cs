using MediatR;

namespace Domain.Models.Event.DomainEvents;

public record BookEventDomainEvent(Ulid EventAttendeeId) : INotification;