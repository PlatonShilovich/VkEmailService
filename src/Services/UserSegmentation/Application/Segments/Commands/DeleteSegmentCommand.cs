using MediatR;

namespace UserSegmentation.Application.Segments.Commands;

public record DeleteSegmentCommand(Guid Id) : IRequest; 