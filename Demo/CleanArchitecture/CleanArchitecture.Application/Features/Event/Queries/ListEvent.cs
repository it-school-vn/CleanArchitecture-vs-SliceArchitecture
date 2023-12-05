using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Message;
using CleanArchitecture.Application.Core.Models;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.Event.DTO;
using CleanArchitecture.Domain.Models.Event.Enum;

namespace CleanArchitecture.Application.Features.Event.Queries;

public static class ListEvent
{
    public class Query : IQuery<OperationResult<Pagination<EventResponse>>>
    {

        public int Page { get; set; } = 1;

        public int Size { get; set; } = Global.PageSize;

        public string? Search { get; set; }

        public required EventType EventType { get; set; }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Page).Must(x => x >= 1)
            .WithMessage("Your page must be greater or equal to 1");


            RuleFor(x => x.Size).Must(x => x >= Global.PageSize && x <= Global.MaxPageSize)
            .WithMessage($"Your page must be greater {Global.PageSize} or equal to {Global.MaxPageSize}");

            RuleFor(x => x.Search)
            .MaximumLength(250)
            .WithMessage("Your search length must be less than 250");

            RuleFor(x => x.EventType).IsInEnum()
            .WithMessage("Your event type must be Event or Appointment");
        }
    }
    public class Handler : IQueryHandler<Query, OperationResult<Pagination<EventResponse>>>
    {
        private readonly IGenericService<EventEntity> _eventService;

        private readonly IMapper _mapper;
        public Handler(IGenericService<EventEntity> eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<OperationResult<Pagination<EventResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            static Expression<Func<EventEntity, bool>> MakePredication(Query request)
            {
                if (string.IsNullOrEmpty(request.Search))
                {
                    return x => x.EventType == request.EventType;
                }

                return x => x.EventType == request.EventType && (x.Title.Contains(request.Search!)
                        || (x.Tags != null && x.Tags.Where(y => y.Name.Contains(request.Search!)).Any()));

            }

            var predication = MakePredication(request);

            var queryResult = await
                _eventService.ListByAsync(predication,
                x => x.OrderByDescending(y => y.Added),
                x => x.Include(y => y.Tags),
                false,
                request.Page,
                request.Size,
                cancellationToken
            );

            var responseItems = queryResult.Data.Select(x => _mapper.Map<EventResponse>(x)).ToList();

            var response = new Pagination<EventResponse>(responseItems, queryResult.TotalCount, queryResult.CurrentPage, queryResult.PageSize);

            return OperationResult.Ok(response);

        }
    }
}