using MediatR;
using Analytics.Services;

namespace Analytics.Application.Events.Commands;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventService _service;
    public DeleteEventCommandHandler(IEventService service) => _service = service;
    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        => await _service.DeleteAsync(request.Id);
} 