using MediatR;
using EmailCampaign.Dtos;

namespace EmailCampaign.Application.Campaigns.Commands;

public record CreateCampaignCommand(CampaignDto Dto) : IRequest<CampaignDto>; 