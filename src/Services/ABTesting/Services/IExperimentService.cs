using ABTesting.Dtos;

namespace ABTesting.Services;

public interface IExperimentService
{
    Task<IEnumerable<ExperimentDto>> GetAllAsync();
    Task<ExperimentDto?> GetByIdAsync(Guid id);
    Task<ExperimentDto> CreateAsync(ExperimentDto dto);
    Task UpdateAsync(Guid id, ExperimentDto dto);
    Task DeleteAsync(Guid id);
    Task StartAsync(Guid id);
    Task StopAsync(Guid id);
    Task<UserAssignmentDto> AssignUserAsync(Guid experimentId, Guid userId);
    Task<object> GetResultsAsync(Guid experimentId);
} 