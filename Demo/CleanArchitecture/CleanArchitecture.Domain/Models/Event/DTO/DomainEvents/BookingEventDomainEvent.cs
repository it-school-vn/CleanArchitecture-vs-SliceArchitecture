using MediatR;

namespace CleanArchitecture.Domain.Models.Event.DTO.DomainEvents;

public record BookEventDomainEvent(Ulid EventAttendeeId) : INotification;