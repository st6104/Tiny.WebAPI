using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.Infrastructure.Abstract.EntityTypeConfigure;
using Tiny.Infrastructure.Abstract.Extensions;

namespace Tiny.Infrastructure.EntityConfigurations;

public class DepartmentEntityConfiguration : EntityTypeConfigurationBase<Department>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department", TinyDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseHiLo();

        builder.HasIndexWithTenantId(x => x.Code).IsUnique();
        builder.Property(x => x.Code).IsRequired();

        builder.Property(x => x.Name);
    }
}
