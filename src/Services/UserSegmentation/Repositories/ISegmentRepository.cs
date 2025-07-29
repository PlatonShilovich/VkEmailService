using UserSegmentation.Entities;

namespace UserSegmentation.Repositories;

public interface ISegmentRepository
{
    Task<IEnumerable<Segment>> GetAllAsync(int page, int pageSize);
    Task<Segment?> GetByIdAsync(Guid id);
    Task<Segment> AddAsync(Segment segment);
    Task UpdateAsync(Segment segment);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<User>> GetSegmentUsersAsync(Guid segmentId, SegmentCriteria criteria, int page, int pageSize);
    Task<int> CountAsync();
} 