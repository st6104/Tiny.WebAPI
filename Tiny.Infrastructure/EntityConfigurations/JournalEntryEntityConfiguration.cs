using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.MultiTenant.EntityTypeConfigure;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryEntityConfiguration : EntityTypeConfigurationBase<JournalEntry>
{
    protected override void ConfigureEntity(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable(nameof(JournalEntry), TinyDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().UseHiLo();

        builder.Property(x => x.DepartmentId)
            .IsRequired();

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired();

        builder.Property(x => x.JournalEntryStatusId)
            .IsRequired();

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.JournalEntryStatusId)
            .IsRequired();

        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey("JournalEntryId")
            .IsRequired();

        builder.Property(x => x.Description).HasDefaultValue(string.Empty);

        builder.Property(x => x.Deleted).HasDefaultValue(false);
    }
}
