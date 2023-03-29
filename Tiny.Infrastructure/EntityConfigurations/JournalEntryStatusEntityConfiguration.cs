using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract;
using Tiny.Infrastructure.Abstract.MultiTenant;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryStatusEntityConfiguration : EntityTypeConfigurationBase<JournalEntryStatus>
{
    public JournalEntryStatusEntityConfiguration(ITenantInfo currentTenant) : base(currentTenant)
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<JournalEntryStatus> builder)
    {
        builder.ToTable(nameof(JournalEntryStatus), TinyContext.DefaultSchema);

        builder.HasKey(x => x.Value);
        builder.Property(x => x.Value)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name);

        builder.HasData(JournalEntryStatus.List);
    }
}
