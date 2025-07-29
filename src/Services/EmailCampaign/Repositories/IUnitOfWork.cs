namespace EmailCampaign.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICampaignRepository Campaigns { get; }
    Task<int> SaveChangesAsync();
} 