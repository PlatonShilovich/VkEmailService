using MediatR;
using EmailCampaign.Services;

namespace EmailCampaign.Application.Campaigns.Commands;

public class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommand>
{
    private readonly ICampaignService _service;
    public DeleteCampaignCommandHandler(ICampaignService service) => _service = service;
    public async Task Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
        => await _service.DeleteAsync(request.Id);
} 