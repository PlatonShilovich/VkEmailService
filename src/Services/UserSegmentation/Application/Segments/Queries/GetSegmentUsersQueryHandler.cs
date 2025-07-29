using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Queries;

public class GetSegmentUsersQueryHandler : IRequestHandler<GetSegmentUsersQuery, IEnumerable<UserDto>>
{
    private readonly ISegmentService _service;
    public GetSegmentUsersQueryHandler(ISegmentService service) => _service = service;
    public async Task<IEnumerable<UserDto>> Handle(GetSegmentUsersQuery request, CancellationToken cancellationToken)
        => await _service.GetSegmentUsersAsync(request.SegmentId, request.Criteria, request.Page, request.PageSize);
} 