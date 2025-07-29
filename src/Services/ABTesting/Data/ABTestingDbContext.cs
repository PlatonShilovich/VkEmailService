using Microsoft.EntityFrameworkCore;
using ABTesting.Entities;

namespace ABTesting.Data;

public class ABTestingDbContext : DbContext
{
    public ABTestingDbContext(DbContextOptions<ABTestingDbContext> options) : base(options) { }

    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<ExperimentVariant> Variants { get; set; }
    public DbSet<UserAssignment> UserAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("abtesting");

        modelBuilder.Entity<Experiment>().ToTable("Experiments");
        modelBuilder.Entity<ExperimentVariant>().ToTable("Variants");
        modelBuilder.Entity<UserAssignment>().ToTable("UserAssignments");
    }
}