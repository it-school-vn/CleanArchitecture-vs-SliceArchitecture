using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Models.Event;
using Domain.Models.Event.DomainEvents;

namespace CleanArchitecture.Application.Features.Event.Commands;
public static class BookEvent
{
    public class Command : ICommand<OperationResult<Ulid>>
    {
        [JsonIgnore]
        public Ulid? Id;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotNull()
            .Must(x => x.HasValue && x.Value != Ulid.Empty)
            .WithMessage("Your event Id canot be null");
        }
    }

    public class Handler : ICommandHandler<Command, OperationResult<Ulid>>
    {
        private readonly IGenericService<EventEntity> _eventService;
        private readonly IGenericService<EventAttendeeEntity> _attendeeService;
        private readonly ICurrentUser _currentUser;

        private readonly IMediator _sender;

        public Handler(IGenericService<EventEntity> eventService, ICurrentUser currentUser, IGenericService<EventAttendeeEntity> attendeeService, IMediator sender)
        {
            _eventService = eventService;
            _currentUser = currentUser;
            _attendeeService = attendeeService;
            _sender = sender;
        }

        public async Task<OperationResult<Ulid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var eventEntity = await _eventService.GetByAsync(x => x.Id == request.Id!.Value, false, cancellationToken)
             ?? throw new NotFoundException($"Not found event with Id ={request.Id}");

            var userId = _currentUser.GetUserId();

            if (eventEntity.OwnerId == userId)
            {
                throw BadRequestException.BadRequest("Not allow to book your event");
            }

            var bookedDateTime = DateTime.UtcNow.AddHours(eventEntity.TimeZone);


            var bookedDate = DateOnly.FromDateTime(bookedDateTime);
            var bookedTime = TimeOnly.FromDateTime(bookedDateTime);

            if (bookedDate < eventEntity.OpenDate
            || (bookedDate == eventEntity.OpenDate && bookedTime < eventEntity.OpenTime))
            {
                throw BadRequestException.BadRequest("This event has not opened yet.");
            }

            if (bookedDate > eventEntity.ClosedDate
            || (bookedDate == eventEntity.ClosedDate && bookedTime > eventEntity.ClosedTime))
            {
                throw BadRequestException.BadRequest("The booking event was closed.");
            }

            if (await _attendeeService.AnyAsync(x => x.AccountId == userId && x.EventId == request.Id!.Value,
             cancellationToken) == true)
            {
                return OperationResult.Accepted();
            }

            var newAttendee = new EventAttendeeEntity
            {
                EventId = request.Id!.Value,
                AccountId = userId,
                Fee = eventEntity.Fee,
            };

            newAttendee = await _attendeeService.InsertAsync(newAttendee, cancellationToken);

            await _sender.Publish(new BookEventDomainEvent(newAttendee.Id), cancellationToken);

            return OperationResult.Ok(newAttendee.Id);

        }

    }
}