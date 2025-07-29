using MediatR;
using ABTesting.Dtos;

namespace ABTesting.Application.Experiments.Queries;

public record GetExperimentByIdQuery(Guid Id) : IRequest<ExperimentDto?>; 