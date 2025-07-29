using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Queries;

public class GetSegmentsQueryHandler : IRequestHandler<GetSegmentsQuery, IEnumerable<SegmentDto>>
{
    private readonly ISegmentService _service;
    public GetSegmentsQueryHandler(ISegmentService service) => _service = service;
    public async Task<IEnumerable<SegmentDto>> Handle(GetSegmentsQuery request, CancellationToken cancellationToken)
        => await _service.GetAllSegmentsAsync(request.Page, request.PageSize);
} 