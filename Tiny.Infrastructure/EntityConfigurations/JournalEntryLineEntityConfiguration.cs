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

        builder.Property("_gLAccountId")
                .HasColumnName("GLAccountId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

        builder.HasOne(x => x.GLAccount)
                .WithMany()
                .HasForeignKey("_gLAccountId")
                .IsRequired();

        builder.Property(x => x.DebitAmount);

        builder.Property(x => x.CreditAmount);

        builder.Property(x => x.Description);
    }
}
