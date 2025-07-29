using Analytics.Dtos;
using Analytics.Entities;
using Analytics.Repositories;
using AutoMapper;

namespace Analytics.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repo;
    private readonly IMapper _mapper;

    public EventService(IEventRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync() => _mapper.Map<IEnumerable<EventDto>>(await _repo.GetAllAsync());

    public async Task<EventDto?> GetByIdAsync(Guid id) => _mapper.Map<EventDto>(await _repo.GetByIdAsync(id));

    public async Task<EventDto> CreateAsync(EventDto dto)
    {
        var entity = _mapper.Map<Event>(dto);
        entity.Id = Guid.NewGuid();
        entity.Timestamp = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity);
        return _mapper.Map<EventDto>(created);
    }

    public async Task UpdateAsync(Guid id, EventDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) throw new Exception("Event not found");
        entity.UserId = dto.UserId;
        entity.CampaignId = dto.CampaignId;
        entity.EventType = dto.EventType;
        entity.Timestamp = dto.Timestamp;
        entity.Metadata = dto.Metadata;
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

    public async Task<IEnumerable<EventDto>> FilterAsync(string eventType, DateTime? from, DateTime? to)
        => _mapper.Map<IEnumerable<EventDto>>(await _repo.FilterAsync(eventType, from, to));

    public async Task<string> ExportAsync(string format) => await _repo.ExportAsync(format);

    public async Task<object> CalculateMetricsAsync(Guid campaignId)
    {
        var events = await _repo.FilterAsync(null, null, null); // Filter by campaignId in real
        var opens = events.Count(e => e.EventType == "open" && e.CampaignId == campaignId);
        var clicks = events.Count(e => e.EventType == "click" && e.CampaignId == campaignId);
        var conversions = events.Count(e => e.EventType == "conversion" && e.CampaignId == campaignId);
        var bounces = events.Count(e => e.EventType == "bounce" && e.CampaignId == campaignId);

        var ctr = opens > 0 ? (double)clicks / opens : 0;
        var cr = clicks > 0 ? (double)conversions / clicks : 0;
        var bounceRate = (opens + clicks + conversions) > 0 ? (double)bounces / (opens + clicks + conversions) : 0;

        return new { CTR = ctr, CR = cr, BounceRate = bounceRate };
    }
}
