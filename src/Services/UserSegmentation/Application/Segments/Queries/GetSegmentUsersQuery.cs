using MediatR;
using UserSegmentation.Dtos;

namespace UserSegmentation.Application.Segments.Queries;

public record GetSegmentUsersQuery(Guid SegmentId, SegmentCriteriaDto Criteria, int Page, int PageSize) : IRequest<IEnumerable<UserDto>>; 