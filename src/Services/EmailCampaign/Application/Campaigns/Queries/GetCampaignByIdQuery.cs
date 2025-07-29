using MediatR;
using EmailCampaign.Dtos;

namespace EmailCampaign.Application.Campaigns.Queries;

public record GetCampaignByIdQuery(Guid Id) : IRequest<CampaignDto?>; 