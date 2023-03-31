using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Infrastructure.Abstract.EntityTypeConfigure;

namespace Tiny.Infrastructure.EntityConfigurations;

public class AccountingTypeEntityConfiguration : EntityTypeConfigurationBase<AccountingType>
{
    protected override void ConfigureEntity(EntityTypeBuilder<AccountingType> builder)
    {
        builder.ToTable("AccountingType", TinyDbContext.DefaultSchema);

        builder.HasKey(entity => entity.Value);

        builder.Property(entity => entity.Value)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(x => x.Name);

        //기본 데이터 채움
        builder.HasData(AccountingType.List);
    }
}
