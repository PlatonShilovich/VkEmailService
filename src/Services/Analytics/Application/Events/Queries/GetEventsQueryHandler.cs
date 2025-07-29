using MediatR;
using Analytics.Dtos;
using Analytics.Services;

namespace Analytics.Application.Events.Queries;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, IEnumerable<EventDto>>
{
    private readonly IEventService _service;
    public GetEventsQueryHandler(IEventService service) => _service = service;
    public async Task<IEnumerable<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        => await _service.GetAllAsync();
} 