using MediatR;
using Analytics.Dtos;

namespace Analytics.Application.Events.Queries;

public record GetEventsQuery() : IRequest<IEnumerable<EventDto>>; 