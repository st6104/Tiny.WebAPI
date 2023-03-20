using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", TinyContext.Default_Schema);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().UseHiLo();

        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Code).IsRequired();

        builder.Property(x => x.Name);
    }
}
