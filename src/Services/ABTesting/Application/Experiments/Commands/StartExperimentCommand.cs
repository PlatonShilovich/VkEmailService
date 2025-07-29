using MediatR;

namespace ABTesting.Application.Experiments.Commands;

public record StartExperimentCommand(Guid Id) : IRequest; 