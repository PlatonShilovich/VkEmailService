using MediatR;
using ABTesting.Dtos;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Queries;

public class GetExperimentsQueryHandler : IRequestHandler<GetExperimentsQuery, IEnumerable<ExperimentDto>>
{
    private readonly IExperimentService _service;
    public GetExperimentsQueryHandler(IExperimentService service) => _service = service;
    public async Task<IEnumerable<ExperimentDto>> Handle(GetExperimentsQuery request, CancellationToken cancellationToken)
        => await _service.GetAllAsync();
} 