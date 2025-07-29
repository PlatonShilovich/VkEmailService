using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Queries;

public class GetSegmentByIdQueryHandler : IRequestHandler<GetSegmentByIdQuery, SegmentDto?>
{
    private readonly ISegmentService _service;
    public GetSegmentByIdQueryHandler(ISegmentService service) => _service = service;
    public async Task<SegmentDto?> Handle(GetSegmentByIdQuery request, CancellationToken cancellationToken)
        => await _service.GetSegmentByIdAsync(request.Id);
} 