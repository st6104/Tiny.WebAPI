using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;
using Tiny.MultiTenant.EntityTypeConfigure;
using Tiny.MultiTenant.Extensions;

namespace Tiny.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : EntityTypeConfigurationBase<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", TinyDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseHiLo();

        builder.HasIndexWithTenantId(x => x.Code).IsUnique();
        builder.Property(x => x.Code).IsRequired();

        builder.Property(x => x.Name);
    }
}
