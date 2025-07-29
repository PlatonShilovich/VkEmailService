using MediatR;

namespace Analytics.Application.Events.Commands;

public record DeleteEventCommand(Guid Id) : IRequest; 