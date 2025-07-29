using AutoMapper;
using ABTesting.Entities;
using ABTesting.Dtos;

namespace ABTesting.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Experiment, ExperimentDto>().ReverseMap();
        CreateMap<ExperimentVariant, ExperimentVariantDto>().ReverseMap();
        CreateMap<UserAssignment, UserAssignmentDto>().ReverseMap();
    }
} 