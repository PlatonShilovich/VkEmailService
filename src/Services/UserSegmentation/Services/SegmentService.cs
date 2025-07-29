using AutoMapper;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using UserSegmentation.Dtos;
using UserSegmentation.Entities;
using UserSegmentation.Repositories;

namespace UserSegmentation.Services;

public class SegmentService : ISegmentService
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;

    public SegmentService(ISegmentRepository segmentRepository, IMapper mapper, IConnectionMultiplexer redis)
    {
        _segmentRepository = segmentRepository;
        _mapper = mapper;
        _redis = redis;
    }

    public async Task<IEnumerable<SegmentDto>> GetAllSegmentsAsync(int page, int pageSize)
    {
        var segments = await _segmentRepository.GetAllAsync(page, pageSize);
        return _mapper.Map<IEnumerable<SegmentDto>>(segments);
    }

    public async Task<SegmentDto?> GetSegmentByIdAsync(Guid id)
    {
        var segment = await _segmentRepository.GetByIdAsync(id);
        return segment == null ? null : _mapper.Map<SegmentDto>(segment);
    }

    public async Task<SegmentDto> CreateSegmentAsync(SegmentDto dto)
    {
        var segment = _mapper.Map<Segment>(dto);
        segment.Id = Guid.NewGuid();
        segment.CreatedAt = DateTime.UtcNow;
        segment.UpdatedAt = DateTime.UtcNow;
        var created = await _segmentRepository.AddAsync(segment);
        return _mapper.Map<SegmentDto>(created);
    }

    public async Task UpdateSegmentAsync(Guid id, SegmentDto dto)
    {
        var segment = await _segmentRepository.GetByIdAsync(id);
        if (segment == null) throw new Exception("Segment not found");
        segment.Name = dto.Name;
        segment.Description = dto.Description;
        segment.Criteria = dto.Criteria;
        segment.UpdatedAt = DateTime.UtcNow;
        await _segmentRepository.UpdateAsync(segment);
    }

    public async Task DeleteSegmentAsync(Guid id)
    {
        await _segmentRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserDto>> GetSegmentUsersAsync(Guid segmentId, SegmentCriteriaDto criteria, int page, int pageSize)
    {
        var db = _redis.GetDatabase();
        var cacheKey = $"segment:{segmentId}:criteria:{JsonSerializer.Serialize(criteria)}:page:{page}:size:{pageSize}";
        var cached = await db.StringGetAsync(cacheKey);
        if (cached.HasValue)
        {
            return JsonSerializer.Deserialize<IEnumerable<UserDto>>(cached!)!;
        }
        // Преобразуем criteria в Entity
        var segmentCriteria = _mapper.Map<SegmentCriteria>(criteria);
        var users = await _segmentRepository.GetSegmentUsersAsync(segmentId, segmentCriteria, page, pageSize);
        var result = _mapper.Map<IEnumerable<UserDto>>(users);
        await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromHours(1));
        return result;
    }

    public async Task<string> ExportSegmentAsync(Guid segmentId, SegmentCriteriaDto criteria, string format)
    {
        var users = await GetSegmentUsersAsync(segmentId, criteria, 1, int.MaxValue);
        if (format.ToLower() == "csv")
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Email,Name,Age,Gender,Location,RegistrationDate,LastActivity");
            foreach (var u in users)
            {
                sb.AppendLine($"{u.Id},{u.Email},{u.Name},{u.Age},{u.Gender},{u.Location},{u.RegistrationDate:o},{u.LastActivity:o}");
            }
            return sb.ToString();
        }
        else // json
        {
            return JsonSerializer.Serialize(users);
        }
    }
} 