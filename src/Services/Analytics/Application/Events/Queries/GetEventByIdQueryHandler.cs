using MediatR;
using Analytics.Dtos;
using Analytics.Services;

namespace Analytics.Application.Events.Queries;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto?>
{
    private readonly IEventService _service;
    public GetEventByIdQueryHandler(IEventService service) => _service = service;
    public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        => await _service.GetByIdAsync(request.Id);
} 