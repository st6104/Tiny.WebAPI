using Tiny.Shared.DomainEntity;
using Tiny.Shared.DomainEvent;

namespace Tiny.Infrastructure.Extensions;

internal static class ModelConfigurationBuilderExtension
{
    public static void SetPreconventions(this ModelConfigurationBuilder builder)
    {
        //IReadOnlyList<IDomainEvent> Type 무시
        //builder.IgnoreAny<IReadOnlyList<IDomainEvent>>();

        builder.Properties<decimal>()
            .HavePrecision(Constraint.Precision.Precision, Constraint.Precision.Scale);

        builder.Properties<string>()
            .HaveMaxLength(Constraint.MaxLength);
    }
}
