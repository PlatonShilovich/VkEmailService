using MediatR;

namespace ABTesting.Application.Experiments.Commands;

public record DeleteExperimentCommand(Guid Id) : IRequest; 