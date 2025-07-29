using Analytics.Entities;

namespace Analytics.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event> AddAsync(Event entity);
    Task UpdateAsync(Event entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Event>> FilterAsync(string eventType, DateTime? from, DateTime? to);
    Task<string> ExportAsync(string format);
} 