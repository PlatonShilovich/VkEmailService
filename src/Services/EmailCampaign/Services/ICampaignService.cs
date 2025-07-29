using EmailCampaign.Dtos;

namespace EmailCampaign.Services;

public interface ICampaignService
{
    Task<IEnumerable<CampaignDto>> GetAllAsync();
    Task<CampaignDto?> GetByIdAsync(Guid id);
    Task<CampaignDto> CreateAsync(CampaignDto dto);
    Task UpdateAsync(Guid id, CampaignDto dto);
    Task DeleteAsync(Guid id);
} 