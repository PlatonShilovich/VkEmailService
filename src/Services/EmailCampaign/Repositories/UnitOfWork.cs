using EmailCampaign.Data;

namespace EmailCampaign.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EmailCampaignDbContext _context;
    public ICampaignRepository Campaigns { get; }
    public UnitOfWork(EmailCampaignDbContext context, ICampaignRepository campaigns)
    {
        _context = context;
        Campaigns = campaigns;
    }
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
} 