using Microsoft.EntityFrameworkCore;
using UserSegmentation.Entities;

namespace UserSegmentation.Data;

public class UserSegmentationDbContext : DbContext
{
    public UserSegmentationDbContext(DbContextOptions<UserSegmentationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Segment> Segments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("usersegmentation");

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Segment>().ToTable("Segments");
    }
}