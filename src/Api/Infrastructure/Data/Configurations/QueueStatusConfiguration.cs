using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Api.Models;

namespace Api.Infrastructure.Data.Configurations;

public class QueueStatusConfiguration : IEntityTypeConfiguration<QueueStatus>
{
    public void Configure(EntityTypeBuilder<QueueStatus> builder)
    {
        builder.HasKey(x => x.TrackingId);
        
        builder.Property(x => x.Status)
               .HasConversion<string>()  // If using enum
               .IsRequired();
        
        builder.Property(x => x.CreatedAt)
               .IsRequired();
               
        builder.Property(x => x.ProcessedAt)
               .IsRequired(false);
               
        builder.Property(x => x.Error)
               .HasMaxLength(500)
               .IsRequired(false);
    }
}