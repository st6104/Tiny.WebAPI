using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.MultiTenant.EntityTypeConfigure;

namespace Tiny.Infrastructure.EntityConfigurations;

public class PostableEntityConfiguration : EntityTypeConfigurationBase<Postable>
{
   protected override void ConfigureEntity(EntityTypeBuilder<Postable> builder)
    {
        builder.ToTable(nameof(Postable), TinyDbContext.DefaultSchema);

        builder.HasKey(x => x.Value);

        builder.Property(x => x.Value)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name);

        builder.HasData(Postable.List);
    }
}
