using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class PostableEntityConfiguration : IEntityTypeConfiguration<Postable>
{
    public void Configure(EntityTypeBuilder<Postable> builder)
    {
        builder.ToTable("Postable");

        builder.HasKey(x => x.Value);

        builder.Property(x => x.Value)
                .HasColumnName("Id")
                .ValueGeneratedNever();

        builder.Property(x => x.Name);

        builder.HasData(Postable.List);
    }
}