using MediatR;
using ABTesting.Dtos;

namespace ABTesting.Application.Experiments.Commands;

public record CreateExperimentCommand(ExperimentDto Dto) : IRequest<ExperimentDto>; 