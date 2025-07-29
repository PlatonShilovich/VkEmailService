using ABTesting.Data;

namespace ABTesting.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ABTestingDbContext _context;
    public IExperimentRepository Experiments { get; }
    public UnitOfWork(ABTestingDbContext context, IExperimentRepository experiments)
    {
        _context = context;
        Experiments = experiments;
    }
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
} 