using MediatR;
using Analytics.Dtos;

namespace Analytics.Application.Events.Queries;

public record GetEventByIdQuery(Guid Id) : IRequest<EventDto?>; 