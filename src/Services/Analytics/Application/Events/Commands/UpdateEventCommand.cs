using MediatR;
using Analytics.Dtos;

namespace Analytics.Application.Events.Commands;

public record UpdateEventCommand(Guid Id, EventDto Dto) : IRequest; 