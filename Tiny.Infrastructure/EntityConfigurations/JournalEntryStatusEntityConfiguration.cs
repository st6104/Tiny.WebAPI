using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryStatusEntityConfiguration : EntityTypeConfigurationBase<JournalEntryStatus>
{
    public JournalEntryStatusEntityConfiguration(ITenantInfo currentTenant) : base(currentTenant)
    {
    }

    public  override void ConfigureEntity(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<JournalEntryStatus> builder)
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
