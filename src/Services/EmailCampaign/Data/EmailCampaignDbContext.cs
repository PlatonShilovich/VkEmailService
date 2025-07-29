using Microsoft.EntityFrameworkCore;
using EmailCampaign.Entities;

namespace EmailCampaign.Data;

public class EmailCampaignDbContext : DbContext
{
    public EmailCampaignDbContext(DbContextOptions<EmailCampaignDbContext> options) : base(options) { }

    public DbSet<Campaign> Campaigns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("emailcampaign");

        modelBuilder.Entity<Campaign>().ToTable("Campaigns");
    }
}