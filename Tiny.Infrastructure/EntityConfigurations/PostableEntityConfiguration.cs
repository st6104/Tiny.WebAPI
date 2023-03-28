using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract;

namespace Tiny.Infrastructure.EntityConfigurations;

public class PostableEntityConfiguration : EntityTypeConfigurationBase<Postable>
{
    public PostableEntityConfiguration(ITenantInfo currentTenant) : base(currentTenant)
    {
    }

    public override void ConfigureEntity(EntityTypeBuilder<Postable> builder)
    {
        builder.ToTable(nameof(Postable), TinyContext.DefaultSchema);

        builder.HasKey(x => x.Value);

        builder.Property(x => x.Value)
                .HasColumnName("Id")
                .ValueGeneratedNever();

        builder.Property(x => x.Name);

        builder.HasData(Postable.List);
    }
}
