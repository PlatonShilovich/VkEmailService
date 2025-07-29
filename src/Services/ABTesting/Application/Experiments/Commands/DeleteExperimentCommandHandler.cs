using MediatR;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class DeleteExperimentCommandHandler : IRequestHandler<DeleteExperimentCommand>
{
    private readonly IExperimentService _service;
    public DeleteExperimentCommandHandler(IExperimentService service) => _service = service;
    public async Task Handle(DeleteExperimentCommand request, CancellationToken cancellationToken)
        => await _service.DeleteAsync(request.Id);
} 