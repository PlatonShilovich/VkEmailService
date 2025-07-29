using EmailCampaign.Dtos;
using EmailCampaign.Entities;
using EmailCampaign.Repositories;
using AutoMapper;
using HandlebarsDotNet;

namespace EmailCampaign.Services;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repo;
    private readonly IMapper _mapper;
    private readonly IHandlebars _handlebars;

    public CampaignService(ICampaignRepository repo, IMapper mapper, IHandlebars handlebars)
    {
        _repo = repo;
        _mapper = mapper;
        _handlebars = handlebars;
    }

    public async Task<IEnumerable<CampaignDto>> GetAllAsync() => _mapper.Map<IEnumerable<CampaignDto>>(await _repo.GetAllAsync());

    public async Task<CampaignDto?> GetByIdAsync(Guid id) => _mapper.Map<CampaignDto>(await _repo.GetByIdAsync(id));

    public async Task<CampaignDto> CreateAsync(CampaignDto dto)
    {
        var entity = _mapper.Map<Campaign>(dto);
        entity.Id = Guid.NewGuid();
        entity.Status = "Draft";

        // Template compilation example
        var template = _handlebars.Compile(entity.Content);
        var rendered = template(new { Name = "TestUser" }); // Test render

        var created = await _repo.AddAsync(entity);
        return _mapper.Map<CampaignDto>(created);
    }

    public async Task UpdateAsync(Guid id, CampaignDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) throw new Exception("Campaign not found");
        entity.Name = dto.Name;
        entity.Subject = dto.Subject;
        entity.Content = dto.Content;
        entity.SegmentId = dto.SegmentId;
        entity.ExperimentId = dto.ExperimentId;
        entity.Status = dto.Status;
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}