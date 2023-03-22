using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryLineEntityConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.ToTable("JournalEntryLine", TinyContext.Default_Schema);

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
