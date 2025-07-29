using MediatR;
using ABTesting.Dtos;

namespace ABTesting.Application.Experiments.Queries;

public record GetExperimentsQuery() : IRequest<IEnumerable<ExperimentDto>>; 