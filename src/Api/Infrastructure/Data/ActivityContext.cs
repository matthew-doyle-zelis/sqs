using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Infrastructure.Data.Configurations;

namespace Api.Infrastructure.Data;

public class ActivityContext : DbContext
{
    public ActivityContext(DbContextOptions<ActivityContext> options) : base(options)
    {
    }

    public required DbSet<Activity> Activities { get; set; }
    public required DbSet<QueueStatus> QueueStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ActivityConfiguration());
        modelBuilder.ApplyConfiguration(new QueueStatusConfiguration());
    }
}