using EmailCampaign.Entities;

namespace EmailCampaign.Repositories;

public interface ICampaignRepository
{
    Task<IEnumerable<Campaign>> GetAllAsync();
    Task<Campaign?> GetByIdAsync(Guid id);
    Task<Campaign> AddAsync(Campaign campaign);
    Task UpdateAsync(Campaign campaign);
    Task DeleteAsync(Guid id);
} 