namespace Tiny.Shared.DomainEvent;

public abstract class DomainEvent : IDomainEvent
{
    private const string GuidNumberFormat = "N";
    public string Id => Guid.NewGuid().ToString(GuidNumberFormat);
}