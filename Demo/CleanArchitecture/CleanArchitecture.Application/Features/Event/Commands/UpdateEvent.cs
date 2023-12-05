using System.ComponentModel;
using System.Text.Json.Serialization;
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

public static class UpdateEvent
{
    [DisplayName("Update event request")]
    public class Command : IOwnerCommand<Ulid?, OperationResult>
    {
        public CreateOrUpdateEventRequest? Request { get; set; }

        [JsonIgnore]
        public Ulid? Id { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).Cascade(CascadeMode.Stop).NotNull()
            .WithMessage("Your event Id cannot be empty");
            RuleFor(x => x.Request).Cascade(CascadeMode.Stop).NotNull()
            .WithMessage("Your request cannot be null")
            .SetInheritanceValidator(x => x.Add(new CreateOrUpdateEventRequestValidator()));

        }
    }

    public class Handler : BaseOwnerCommandHandler<Ulid?, Command, OperationResult>
    {
        private readonly IGenericService<EventEntity> _eventService;

        private readonly IMapper _mapper;

        public Handler(IGenericService<EventEntity> eventService,
         ICurrentUser currentUser, IMapper mapper) : base(currentUser)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        protected override async Task<IOwner> GetItemOwner(Command request, CancellationToken cancellationToken)
        {
            return await _eventService.GetByAsync(x => x.Id == request.Id!.Value, true, cancellationToken);
        }

        protected override async Task<OperationResult> HandleMain(Command request, IOwner owner, CancellationToken cancellationToken)
        {
            var eventEntity = owner as EventEntity;

            _mapper.Map(request.Request, eventEntity);

            await _eventService.UpdateAsync(eventEntity!, cancellationToken);

            return OperationResult.Accepted();
        }
    }
}