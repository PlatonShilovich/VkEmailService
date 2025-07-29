using Analytics.Data;

namespace Analytics.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AnalyticsDbContext _context;
    public IEventRepository Events { get; }
    public UnitOfWork(AnalyticsDbContext context, IEventRepository events)
    {
        _context = context;
        Events = events;
    }
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
} 