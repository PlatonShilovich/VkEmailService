using Analytics.Dtos;

namespace Analytics.Services;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GetAllAsync();
    Task<EventDto?> GetByIdAsync(Guid id);
    Task<EventDto> CreateAsync(EventDto dto);
    Task UpdateAsync(Guid id, EventDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<EventDto>> FilterAsync(string eventType, DateTime? from, DateTime? to);
    Task<string> ExportAsync(string format);
    Task<object> CalculateMetricsAsync(Guid campaignId); // Добавленный метод
}