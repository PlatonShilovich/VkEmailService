using MediatR;
using EmailCampaign.Dtos;

namespace EmailCampaign.Application.Campaigns.Queries;

public record GetCampaignsQuery() : IRequest<IEnumerable<CampaignDto>>; 