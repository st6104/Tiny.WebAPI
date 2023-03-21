using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class AccountingTypeEntityConfiguration : IEntityTypeConfiguration<AccountingType>
{
    public void Configure(EntityTypeBuilder<AccountingType> builder)
    {
        builder.ToTable("AccountingType", TinyContext.Default_Schema);

        builder.HasKey(entity => entity.Value);

        builder.Property(entity => entity.Value)
                .HasColumnName("Id")
                .ValueGeneratedNever();

        builder.Property(x => x.Name);

        //기본 데이터 채움
        builder.HasData(AccountingType.List);
    }
}