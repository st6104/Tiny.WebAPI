namespace Tiny.Shared.Exceptions;

/// <summary>
/// 엔티티id를 찾을수 없을때 발생하는 예외
/// </summary>
public sealed class EntityIdNotFoundException : Exception
{
    private readonly bool _assginedMessage;
    
    public long FailedId { get; }
    public override string Message => _assginedMessage ? base.Message : $"EntityId({FailedId}) Not Found";


    public EntityIdNotFoundException(long failedId, string? message = null, Exception? innerException = null) : base(
        message, innerException)
    {
        FailedId = failedId;
        _assginedMessage = !string.IsNullOrWhiteSpace(message);
    }
}
