using MediatR;
using ABTesting.Dtos;

namespace ABTesting.Application.Experiments.Commands;

public record AssignUserCommand(Guid ExperimentId, Guid UserId) : IRequest<UserAssignmentDto>; 