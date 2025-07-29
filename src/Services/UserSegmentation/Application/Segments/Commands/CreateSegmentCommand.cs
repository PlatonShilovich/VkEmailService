using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Commands;

public record CreateSegmentCommand(SegmentDto Dto) : IRequest<SegmentDto>; 