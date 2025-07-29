using ABTesting.Dtos;
using ABTesting.Entities;
using ABTesting.Repositories;
using AutoMapper;
using StackExchange.Redis;
using System.Text.Json;
using MathNet.Numerics.Statistics; // Добавить это

namespace ABTesting.Services;

public class ExperimentService : IExperimentService
{
    private readonly IExperimentRepository _repo;
    private readonly IMapper _mapper;
    private readonly IConnectionMultiplexer _redis;

    public ExperimentService(IExperimentRepository repo, IMapper mapper, IConnectionMultiplexer redis)
    {
        _repo = repo;
        _mapper = mapper;
        _redis = redis;
    }

    public async Task<IEnumerable<ExperimentDto>> GetAllAsync() => _mapper.Map<IEnumerable<ExperimentDto>>(await _repo.GetAllAsync());

    public async Task<ExperimentDto?> GetByIdAsync(Guid id) => _mapper.Map<ExperimentDto>(await _repo.GetByIdAsync(id));

    public async Task<ExperimentDto> CreateAsync(ExperimentDto dto)
    {
        var entity = _mapper.Map<Experiment>(dto);
        entity.Id = Guid.NewGuid();
        entity.Status = "Draft";
        entity.StartDate = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity);
        return _mapper.Map<ExperimentDto>(created);
    }

    public async Task UpdateAsync(Guid id, ExperimentDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) throw new Exception("Experiment not found");
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.TrafficSplit = dto.TrafficSplit;
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

    public async Task StartAsync(Guid id) => await _repo.StartAsync(id);

    public async Task StopAsync(Guid id) => await _repo.StopAsync(id);

    public async Task<UserAssignmentDto> AssignUserAsync(Guid experimentId, Guid userId)
    {
        var db = _redis.GetDatabase();
        var cacheKey = $"assignment:{experimentId}:{userId}";
        var cached = await db.StringGetAsync(cacheKey);
        if (cached.HasValue)
            return JsonSerializer.Deserialize<UserAssignmentDto>(cached!)!;

        var exp = await _repo.GetByIdAsync(experimentId);
        if (exp == null) throw new Exception("Experiment not found");

        // Random distribution
        var totalWeight = exp.Variants.Sum(v => v.Weight);
        var rand = new Random().NextDouble() * totalWeight;
        ExperimentVariant selected = null!;
        double cumulative = 0;
        foreach (var v in exp.Variants)
        {
            cumulative += v.Weight;
            if (rand <= cumulative)
            {
                selected = v;
                break;
            }
        }

        var assignment = new UserAssignment { Id = Guid.NewGuid(), UserId = userId, ExperimentId = experimentId, VariantId = selected.Id, AssignedAt = DateTime.UtcNow };
        await _repo.AssignUserAsync(experimentId, userId); // Save to DB
        var dto = _mapper.Map<UserAssignmentDto>(assignment);
        await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(dto), TimeSpan.FromHours(1));
        return dto;
    }

    public async Task<object> GetResultsAsync(Guid experimentId)
    {
        var assignments = await _repo.GetAssignmentsAsync(experimentId);
        var stats = assignments.GroupBy(a => a.VariantId).Select(g => new { VariantId = g.Key, Count = g.Count() }).ToList();

        // Простая статистическая значимость без ChiSquare
        if (stats.Count >= 2)
        {
            var observed = stats.Select(s => (double)s.Count).ToArray();
            var total = observed.Sum();
            var expected = observed.Select(_ => total / observed.Length).ToArray();
            
            // Простой расчет chi-square
            var chiSquare = 0.0;
            for (int i = 0; i < observed.Length; i++)
            {
                chiSquare += Math.Pow(observed[i] - expected[i], 2) / expected[i];
            }
            
            // Упрощенная оценка значимости
            var significant = chiSquare > 3.84; // критическое значение для p=0.05, df=1
            
            return new { Stats = stats, ChiSquare = chiSquare, Significant = significant };
        }
        return new { Stats = stats };
    }
}
