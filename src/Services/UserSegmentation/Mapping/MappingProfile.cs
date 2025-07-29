using AutoMapper;
using UserSegmentation.Entities;
using UserSegmentation.Dtos;

namespace UserSegmentation.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Segment, SegmentDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
} 