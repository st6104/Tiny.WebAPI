namespace Tiny.Infrastructure.Abstract.MultiTenant;

public interface ITenantInfo
{
    public string Id { get; }
    public string Name { get; }
    public string ConnectionString { get; }
}
