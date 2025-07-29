using MediatR;
using ABTesting.Dtos;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Queries;

public class GetExperimentByIdQueryHandler : IRequestHandler<GetExperimentByIdQuery, ExperimentDto?>
{
    private readonly IExperimentService _service;
    public GetExperimentByIdQueryHandler(IExperimentService service) => _service = service;
    public async Task<ExperimentDto?> Handle(GetExperimentByIdQuery request, CancellationToken cancellationToken)
        => await _service.GetByIdAsync(request.Id);
} 