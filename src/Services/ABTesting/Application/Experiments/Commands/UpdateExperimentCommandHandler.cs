using MediatR;
using ABTesting.Dtos;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class UpdateExperimentCommandHandler : IRequestHandler<UpdateExperimentCommand>
{
    private readonly IExperimentService _service;
    public UpdateExperimentCommandHandler(IExperimentService service) => _service = service;
    public async Task Handle(UpdateExperimentCommand request, CancellationToken cancellationToken)
        => await _service.UpdateAsync(request.Id, request.Dto);
} 