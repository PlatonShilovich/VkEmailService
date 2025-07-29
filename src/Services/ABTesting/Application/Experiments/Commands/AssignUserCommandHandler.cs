using MediatR;
using ABTesting.Dtos;
using ABTesting.Services;

namespace ABTesting.Application.Experiments.Commands;

public class AssignUserCommandHandler : IRequestHandler<AssignUserCommand, UserAssignmentDto>
{
    private readonly IExperimentService _service;
    public AssignUserCommandHandler(IExperimentService service) => _service = service;
    public async Task<UserAssignmentDto> Handle(AssignUserCommand request, CancellationToken cancellationToken)
        => await _service.AssignUserAsync(request.ExperimentId, request.UserId);
} 