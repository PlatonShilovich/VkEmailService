using MediatR;

namespace EmailCampaign.Application.Campaigns.Commands;

public record DeleteCampaignCommand(Guid Id) : IRequest; 