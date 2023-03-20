using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiny.Domain.AggregateModels.JournalEntryAggregate;

namespace Tiny.Infrastructure.EntityConfigurations;

public class JournalEntryEntityConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().UseHiLo();

        builder.Property("_departmentId")
                .HasColumnName("DepartmentId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

        builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey("_departmentId")
                .IsRequired();

        builder.Property("_journalEntryStatusId")
                .HasColumnName("JournalEntryStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

        builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey("_journalEntryStatusId")
                .IsRequired();

        builder.HasMany(x => x.Lines)
                .WithOne()
                .HasForeignKey("JournalEntryId")
                .IsRequired();

        builder.Property(x => x.Description);
    }
}
