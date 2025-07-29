using MediatR;
using ABTesting.Dtos;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class CreateExperimentCommandHandler : IRequestHandler<CreateExperimentCommand, ExperimentDto>
{
    private readonly IExperimentService _service;
    public CreateExperimentCommandHandler(IExperimentService service) => _service = service;
    public async Task<ExperimentDto> Handle(CreateExperimentCommand request, CancellationToken cancellationToken)
        => await _service.CreateAsync(request.Dto);
} 