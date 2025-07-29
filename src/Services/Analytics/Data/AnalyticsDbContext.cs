using Microsoft.EntityFrameworkCore;
using Analytics.Entities;

namespace Analytics.Data;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("analytics");

        modelBuilder.Entity<Event>().ToTable("Events");
    }
}