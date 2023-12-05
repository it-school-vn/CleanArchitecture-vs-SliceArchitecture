using AutoMapper;
using CleanArchitecture.Domain.Models.Event.DTO;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Models.Event;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrUpdateEventRequest, EventEntity>()
         .ForMember(x => x.Tags, y => y.MapFrom(t => t.Tags != null && t.Tags.Any() ? t.Tags.Select(k => new WithName(k)).ToArray() : null));

        CreateMap<EventEntity, EventResponse>()
        .ForMember(x => x.Tags, y => y.MapFrom(s => s.Tags != null ? s.Tags.Select(x => x.Name) : null));
    }
}