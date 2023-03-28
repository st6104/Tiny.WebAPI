using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryLineEntityConfiguration : EntityTypeConfigurationBase<JournalEntryLine>
{
    public JournalEntryLineEntityConfiguration(ITenantInfo currentTenant) : base(currentTenant)
    {
    }

    public override void ConfigureEntity(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.ToTable(nameof(JournalEntryLine), TinyContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.GLAccountId)
                .IsRequired();

        builder.HasOne(x => x.GLAccount)
                .WithMany()
                .HasForeignKey(x => x.GLAccountId)
                .IsRequired();

        builder.Property(x => x.DebitAmount).HasDefaultValue(decimal.Zero);

        builder.Property(x => x.CreditAmount).HasDefaultValue(decimal.Zero);

        builder.Property(x => x.Description).HasDefaultValue(string.Empty);
    }
}
