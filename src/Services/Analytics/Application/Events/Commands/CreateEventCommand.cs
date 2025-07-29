using MediatR;
using Analytics.Dtos;

namespace Analytics.Application.Events.Commands;

public record CreateEventCommand(EventDto Dto) : IRequest<EventDto>; 