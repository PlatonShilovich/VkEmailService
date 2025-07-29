using MediatR;
using EmailCampaign.Dtos;
using EmailCampaign.Services;

namespace EmailCampaign.Application.Campaigns.Commands;

public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, CampaignDto>
{
    private readonly ICampaignService _service;
    public CreateCampaignCommandHandler(ICampaignService service) => _service = service;
    public async Task<CampaignDto> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        => await _service.CreateAsync(request.Dto);
} 