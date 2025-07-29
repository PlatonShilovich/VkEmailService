using AutoMapper;
using Analytics.Entities;
using Analytics.Dtos;

namespace Analytics.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>().ReverseMap();
    }
} 