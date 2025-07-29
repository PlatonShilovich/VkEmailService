using MediatR;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class StartExperimentCommandHandler : IRequestHandler<StartExperimentCommand>
{
    private readonly IExperimentService _service;
    public StartExperimentCommandHandler(IExperimentService service) => _service = service;
    public async Task Handle(StartExperimentCommand request, CancellationToken cancellationToken)
        => await _service.StartAsync(request.Id);
} 