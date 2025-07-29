using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Commands;

public record UpdateSegmentCommand(Guid Id, SegmentDto Dto) : IRequest; 