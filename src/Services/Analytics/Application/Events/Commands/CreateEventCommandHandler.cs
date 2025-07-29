using MediatR;
using Analytics.Dtos;
using Analytics.Services;

namespace Analytics.Application.Events.Commands;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventService _service;
    public CreateEventCommandHandler(IEventService service) => _service = service;
    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        => await _service.CreateAsync(request.Dto);
} 