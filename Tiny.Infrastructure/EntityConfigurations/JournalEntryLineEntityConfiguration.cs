using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.EntityTypeConfigure;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryLineEntityConfiguration : EntityTypeConfigurationBase<JournalEntryLine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.ToTable(nameof(JournalEntryLine), TinyDbContext.DefaultSchema);

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
