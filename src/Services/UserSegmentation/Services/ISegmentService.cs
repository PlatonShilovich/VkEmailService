using UserSegmentation.Dtos;
using UserSegmentation.Entities;

namespace UserSegmentation.Services;

public interface ISegmentService
{
    Task<IEnumerable<SegmentDto>> GetAllSegmentsAsync(int page, int pageSize);
    Task<SegmentDto?> GetSegmentByIdAsync(Guid id);
    Task<SegmentDto> CreateSegmentAsync(SegmentDto dto);
    Task UpdateSegmentAsync(Guid id, SegmentDto dto);
    Task DeleteSegmentAsync(Guid id);
    Task<IEnumerable<UserDto>> GetSegmentUsersAsync(Guid segmentId, SegmentCriteriaDto criteria, int page, int pageSize);
    Task<string> ExportSegmentAsync(Guid segmentId, SegmentCriteriaDto criteria, string format);
} 