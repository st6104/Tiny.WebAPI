using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Infrastructure.Abstract;
using Tiny.Infrastructure.Abstract.MultiTenant;

namespace Tiny.Infrastructure.EntityConfigurations;

public class AccountingTypeEntityConfiguration : EntityTypeConfigurationBase<AccountingType>
{
    public AccountingTypeEntityConfiguration(ITenantInfo tenantInfo) : base(tenantInfo)
    {
    }

    protected override void ConfigureEntity(EntityTypeBuilder<AccountingType> builder)
    {
        builder.ToTable("AccountingType", TinyContext.DefaultSchema);

        builder.HasKey(entity => entity.Value);

        builder.Property(entity => entity.Value)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name);

        //기본 데이터 채움
        builder.HasData(AccountingType.List);
    }
}
