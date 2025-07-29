namespace Analytics.Repositories;

public interface IUnitOfWork : IDisposable
{
    IEventRepository Events { get; }
    Task<int> SaveChangesAsync();
} 