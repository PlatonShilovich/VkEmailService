using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Queries;

public record GetSegmentByIdQuery(Guid Id) : IRequest<SegmentDto?>; 