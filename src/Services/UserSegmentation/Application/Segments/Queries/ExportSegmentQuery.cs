using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Queries;

public record ExportSegmentQuery(Guid SegmentId, SegmentCriteriaDto Criteria, string Format) : IRequest<string>; 