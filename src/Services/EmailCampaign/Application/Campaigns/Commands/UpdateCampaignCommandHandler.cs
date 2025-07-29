 using MediatR;
using EmailCampaign.Dtos;
using EmailCampaign.Services;

namespace EmailCampaign.Application.Campaigns.Commands;

public class UpdateCampaignCommandHandler : IRequestHandler<UpdateCampaignCommand>
{
    private readonly ICampaignService _service;
    public UpdateCampaignCommandHandler(ICampaignService service) => _service = service;
    public async Task Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
        => await _service.UpdateAsync(request.Id, request.Dto);
}