using Analytics.Entities;
using Analytics.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Analytics.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AnalyticsDbContext _context;
    public EventRepository(AnalyticsDbContext context) => _context = context;
    public async Task<IEnumerable<Event>> GetAllAsync() => await _context.Events.ToListAsync();
    public async Task<Event?> GetByIdAsync(Guid id) => await _context.Events.FindAsync(id);
    public async Task<Event> AddAsync(Event entity) { _context.Events.Add(entity); await _context.SaveChangesAsync(); return entity; }
    public async Task UpdateAsync(Event entity) { _context.Events.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(Guid id) { var e = await _context.Events.FindAsync(id); if (e != null) { _context.Events.Remove(e); await _context.SaveChangesAsync(); } }
    public async Task<IEnumerable<Event>> FilterAsync(string eventType, DateTime? from, DateTime? to)
    {
        var query = _context.Events.AsQueryable();
        if (!string.IsNullOrEmpty(eventType)) query = query.Where(e => e.EventType == eventType);
        if (from.HasValue) query = query.Where(e => e.Timestamp >= from);
        if (to.HasValue) query = query.Where(e => e.Timestamp <= to);
        return await query.ToListAsync();
    }
    public async Task<string> ExportAsync(string format)
    {
        var events = await _context.Events.ToListAsync();
        if (format.ToLower() == "csv")
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,UserId,CampaignId,EventType,Timestamp,Metadata");
            foreach (var e in events)
                sb.AppendLine($"{e.Id},{e.UserId},{e.CampaignId},{e.EventType},{e.Timestamp:o},{e.Metadata}");
            return sb.ToString();
        }
        else
        {
            return System.Text.Json.JsonSerializer.Serialize(events);
        }
    }
} 