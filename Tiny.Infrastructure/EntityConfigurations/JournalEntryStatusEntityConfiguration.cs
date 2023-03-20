using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryStatusEntityConfiguration : IEntityTypeConfiguration<JournalEntryStatus>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<JournalEntryStatus> builder)
    {
        builder.ToTable("JournalEntryStatus", TinyContext.Default_Schema);

        builder.HasKey(x => x.Value);
        builder.Property(x => x.Value)
                    .HasColumnName("Id")
                    .ValueGeneratedNever();

        builder.Property(x => x.Name);

        builder.HasData(JournalEntryStatus.List);
    }
}