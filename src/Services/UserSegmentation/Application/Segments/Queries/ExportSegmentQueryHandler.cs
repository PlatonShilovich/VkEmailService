using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Queries;

public class ExportSegmentQueryHandler : IRequestHandler<ExportSegmentQuery, string>
{
    private readonly ISegmentService _service;
    public ExportSegmentQueryHandler(ISegmentService service) => _service = service;
    public async Task<string> Handle(ExportSegmentQuery request, CancellationToken cancellationToken)
        => await _service.ExportSegmentAsync(request.SegmentId, request.Criteria, request.Format);
} 