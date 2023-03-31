namespace Tiny.Shared.DomainEntity;

public interface ISoftDeletable
{
    bool Deleted { get; }

    bool TryMarkAsDelete();
}
