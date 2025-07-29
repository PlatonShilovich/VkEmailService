using MediatR;

namespace ABTesting.Application.Experiments.Queries;

public record GetExperimentResultsQuery(Guid ExperimentId) : IRequest<object>; 