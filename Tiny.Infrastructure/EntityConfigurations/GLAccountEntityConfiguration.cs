using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

// GLAccount 엔티티 모델 빌드 클래스
public class GLAccountEntityConfiguration : IEntityTypeConfiguration<GLAccount>
{
    public void Configure(EntityTypeBuilder<GLAccount> builder)
    {
        // 테이블 이름 설정
        builder.ToTable("GLAccount", TinyContext.Default_Schema);

        // Id 프로퍼티를 PK로 설정
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().UseHiLo();

        // Code 프로퍼티를 고유값(Unique)으로 설정
        builder.HasIndex(x => new { x.Code, x.Name }).IsUnique();

        builder.Property(x => x.Code).HasMaxLength(GLAccount.CodeLength).IsRequired();

        // Name 프로퍼티를 문자열길이를 최대200자로 설정하고 NotNull로 설정
        builder.Property(x => x.Name).HasMaxLength(GLAccount.NameLength).IsRequired();

        // _postableId 멤버변수를 NotNull로 설정
        builder.Property<int>("_postableId").HasColumnName("PostableId")
                        .UsePropertyAccessMode(PropertyAccessMode.Field)
                        .IsRequired();

        builder.HasOne(x => x.Postable)
                .WithMany()
                .HasForeignKey("_postableId")
                .IsRequired();

        // _accountTypeId 멤버변수를 NotNull로 설정
        builder.Property<int>("_accountingTypeId").HasColumnName("AccountTypeId")
                        .UsePropertyAccessMode(PropertyAccessMode.Field)
                        .IsRequired();

        builder.HasOne(x => x.AccountingType)
                .WithMany()
                .HasForeignKey("_accountingTypeId")
                .IsRequired();

        // Balance 필드를 기본값을 0으로 설정하고 NotNull로 설정
        builder.Property(x => x.Balance)
                .HasDefaultValue(decimal.Zero);
    }
}
