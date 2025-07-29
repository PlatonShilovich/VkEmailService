using MediatR;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class StopExperimentCommandHandler : IRequestHandler<StopExperimentCommand>
{
    private readonly IExperimentService _service;
    public StopExperimentCommandHandler(IExperimentService service) => _service = service;
    public async Task Handle(StopExperimentCommand request, CancellationToken cancellationToken)
        => await _service.StopAsync(request.Id);
} 