using MediatR;
using EmailCampaign.Dtos;
using EmailCampaign.Services;

namespace EmailCampaign.Application.Campaigns.Queries;

public class GetCampaignByIdQueryHandler : IRequestHandler<GetCampaignByIdQuery, CampaignDto?>
{
    private readonly ICampaignService _service;
    public GetCampaignByIdQueryHandler(ICampaignService service) => _service = service;
    public async Task<CampaignDto?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        => await _service.GetByIdAsync(request.Id);
} 