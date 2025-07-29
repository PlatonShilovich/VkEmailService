using MediatR;

namespace ABTesting.Application.Experiments.Commands;

public record StopExperimentCommand(Guid Id) : IRequest; 