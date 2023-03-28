using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.MultiTenant;
using Tiny.Infrastructure.Abstract;

namespace Tiny.Infrastructure.EntityConfigurations;

public class DepartmentEntityConfiguration : EntityTypeConfigurationBase<Department>
{
    public DepartmentEntityConfiguration(ITenantInfo currentTenant) : base(currentTenant)
    {
    }

    public override void ConfigureEntity(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department", TinyContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseHiLo();

        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Code).IsRequired();

        builder.Property(x => x.Name);
    }
}
