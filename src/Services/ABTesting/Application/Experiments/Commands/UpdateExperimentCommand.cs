using MediatR;
using ABTesting.Dtos;

namespace ABTesting.Application.Experiments.Commands;

public record UpdateExperimentCommand(Guid Id, ExperimentDto Dto) : IRequest; 