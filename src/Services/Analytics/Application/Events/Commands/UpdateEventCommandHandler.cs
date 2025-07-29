using MediatR;
using Analytics.Dtos;
using Analytics.Services;

namespace Analytics.Application.Events.Commands;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IEventService _service;
    public UpdateEventCommandHandler(IEventService service) => _service = service;
    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        => await _service.UpdateAsync(request.Id, request.Dto);
} 