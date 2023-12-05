using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.Event.DTO;

namespace CleanArchitecture.Application.Features.Event.Queries;

public static class GetEvent
{
    public class Query : IQuery<OperationResult<EventResponse>>
    {
        [JsonIgnore]
        public Ulid? Id;
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotNull()
            .WithMessage("Your Id cannot be null or empty");

        }
    }
    public class Handler : IQueryHandler<Query, OperationResult<EventResponse>>
    {
        private readonly IGenericService<EventEntity> _eventService;

        private readonly IMapper _mapper;
        public Handler(IGenericService<EventEntity> eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<OperationResult<EventResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var item = await _eventService.GetByAsync(x => x.Id == request.Id!.Value, false, cancellationToken);

            if (item is null)
            {
                throw new NotFoundException($"Not found event with Id = {request.Id}");
            }

            return OperationResult.Ok(_mapper.Map<EventResponse>(item));
        }
    }
}