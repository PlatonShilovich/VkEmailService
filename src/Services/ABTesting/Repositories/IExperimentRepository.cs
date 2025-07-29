using ABTesting.Entities;

namespace ABTesting.Repositories;

public interface IExperimentRepository
{
    Task<IEnumerable<Experiment>> GetAllAsync();
    Task<Experiment?> GetByIdAsync(Guid id);
    Task<Experiment> AddAsync(Experiment experiment);
    Task UpdateAsync(Experiment experiment);
    Task DeleteAsync(Guid id);
    Task StartAsync(Guid id);
    Task StopAsync(Guid id);
    Task<IEnumerable<UserAssignment>> GetAssignmentsAsync(Guid experimentId);
    Task<UserAssignment> AssignUserAsync(Guid experimentId, Guid userId);
    Task<object> GetResultsAsync(Guid experimentId);
} 