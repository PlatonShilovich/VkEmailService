using AutoMapper;
using EmailCampaign.Entities;
using EmailCampaign.Dtos;

namespace EmailCampaign.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Campaign, CampaignDto>().ReverseMap();
    }
} 