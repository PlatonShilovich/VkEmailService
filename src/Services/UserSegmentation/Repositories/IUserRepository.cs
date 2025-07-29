using UserSegmentation.Entities;

namespace UserSegmentation.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<User>> FilterAsync(SegmentCriteria criteria, int page, int pageSize);
    Task<int> CountAsync();
} 