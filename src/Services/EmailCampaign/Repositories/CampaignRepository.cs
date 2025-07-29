using EmailCampaign.Entities;
using EmailCampaign.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailCampaign.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly EmailCampaignDbContext _context;
    public CampaignRepository(EmailCampaignDbContext context) => _context = context;
    public async Task<IEnumerable<Campaign>> GetAllAsync() => await _context.Campaigns.ToListAsync();
    public async Task<Campaign?> GetByIdAsync(Guid id) => await _context.Campaigns.FindAsync(id);
    public async Task<Campaign> AddAsync(Campaign campaign) { _context.Campaigns.Add(campaign); await _context.SaveChangesAsync(); return campaign; }
    public async Task UpdateAsync(Campaign campaign) { _context.Campaigns.Update(campaign); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(Guid id) { var c = await _context.Campaigns.FindAsync(id); if (c != null) { _context.Campaigns.Remove(c); await _context.SaveChangesAsync(); } }
} 