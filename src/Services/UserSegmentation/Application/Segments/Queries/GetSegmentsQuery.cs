using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Queries;

public record GetSegmentsQuery(int Page, int PageSize) : IRequest<IEnumerable<SegmentDto>>; 