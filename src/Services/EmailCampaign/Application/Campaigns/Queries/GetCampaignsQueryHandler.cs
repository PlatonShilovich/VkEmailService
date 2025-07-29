using MediatR;
using EmailCampaign.Dtos;
using EmailCampaign.Services;

namespace EmailCampaign.Application.Campaigns.Queries;

public class GetCampaignsQueryHandler : IRequestHandler<GetCampaignsQuery, IEnumerable<CampaignDto>>
{
    private readonly ICampaignService _service;
    public GetCampaignsQueryHandler(ICampaignService service) => _service = service;
    public async Task<IEnumerable<CampaignDto>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
        => await _service.GetAllAsync();
} 