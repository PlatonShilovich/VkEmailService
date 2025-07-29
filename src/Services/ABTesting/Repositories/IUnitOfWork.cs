namespace ABTesting.Repositories;

public interface IUnitOfWork : IDisposable
{
    IExperimentRepository Experiments { get; }
    Task<int> SaveChangesAsync();
} 