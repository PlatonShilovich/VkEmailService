using MediatR;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Queries;

public class GetExperimentResultsQueryHandler : IRequestHandler<GetExperimentResultsQuery, object>
{
    private readonly IExperimentService _service;
    public GetExperimentResultsQueryHandler(IExperimentService service) => _service = service;
    public async Task<object> Handle(GetExperimentResultsQuery request, CancellationToken cancellationToken)
        => await _service.GetResultsAsync(request.ExperimentId);
} 