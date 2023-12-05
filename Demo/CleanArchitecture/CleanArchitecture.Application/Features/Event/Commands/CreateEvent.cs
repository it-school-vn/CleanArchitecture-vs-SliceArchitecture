using System.ComponentModel;
using AutoMapper;
using FluentValidation;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Application.Features.Event.Validators;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.Event.DTO;

namespace CleanArchitecture.Application.Features.Event.Commands;

public static class CreateEvent
{
    [DisplayName("Create event request")]
    public class Command : ICommand<OperationResult<Ulid>>
    {
        public CreateOrUpdateEventRequest? Request { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Request).Cascade(CascadeMode.Stop).NotNull()
            .WithMessage("Your request cannot be null")
            .SetInheritanceValidator(x => x.Add(new CreateOrUpdateEventRequestValidator()));

        }
    }

    public class Handler : ICommandHandler<Command, OperationResult<Ulid>>
    {
        private readonly IGenericService<EventEntity> _eventService;

        private readonly ICurrentUser _currentUser;

        private readonly IMapper _mapper;

        public Handler(IGenericService<EventEntity> eventService, ICurrentUser currentUser, IMapper mapper)
        {
            _eventService = eventService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<OperationResult<Ulid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var eventEntity = _mapper.Map<EventEntity>(request.Request);

            eventEntity.OwnerId = _currentUser.GetUserId();

            eventEntity = await _eventService.InsertAsync(eventEntity, cancellationToken);

            return OperationResult.Ok(eventEntity.Id);
        }
    }
}