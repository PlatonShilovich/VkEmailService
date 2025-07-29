using Microsoft.EntityFrameworkCore;
using UserSegmentation.Data;
using UserSegmentation.Entities;

namespace UserSegmentation.Repositories;

public class SegmentRepository : ISegmentRepository
{
    private readonly UserSegmentationDbContext _context;
    private readonly IUserRepository _userRepository;

    public SegmentRepository(UserSegmentationDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Segment>> GetAllAsync(int page, int pageSize)
        => await _context.Segments.OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<Segment?> GetByIdAsync(Guid id)
        => await _context.Segments.FindAsync(id);

    public async Task<Segment> AddAsync(Segment segment)
    {
        _context.Segments.Add(segment);
        await _context.SaveChangesAsync();
        return segment;
    }

    public async Task UpdateAsync(Segment segment)
    {
        _context.Segments.Update(segment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var segment = await _context.Segments.FindAsync(id);
        if (segment != null)
        {
            _context.Segments.Remove(segment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> GetSegmentUsersAsync(Guid segmentId, SegmentCriteria criteria, int page, int pageSize)
    {
        // Получаем сегмент, парсим criteria если нужно
        // Здесь можно добавить логику проверки принадлежности пользователя сегменту
        return await _userRepository.FilterAsync(criteria, page, pageSize);
    }

    public async Task<int> CountAsync()
        => await _context.Segments.CountAsync();
} 