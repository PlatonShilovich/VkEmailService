using MediatR;
using EmailCampaign.Dtos;

namespace EmailCampaign.Application.Campaigns.Commands;

public record UpdateCampaignCommand(Guid Id, CampaignDto Dto) : IRequest; 