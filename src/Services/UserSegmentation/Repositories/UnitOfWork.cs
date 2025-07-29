using UserSegmentation.Data;

namespace UserSegmentation.Repositories;

public class UnitOfWork : IDisposable
{
    private readonly UserSegmentationDbContext _context;
    public ISegmentRepository Segments { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(UserSegmentationDbContext context, ISegmentRepository segments, IUserRepository users)
    {
        _context = context;
        Segments = segments;
        Users = users;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
} 