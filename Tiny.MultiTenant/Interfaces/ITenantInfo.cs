namespace Tiny.MultiTenant.Interfaces;

public interface ITenantInfo
{
    string Id { get; }
    string Name { get; }
    string ConnectionString { get; }
    bool IsActive { get; }

    void ChangeName(string name);
    void ChangeConnectionString(string connectionString);
    void Active();
    void Inactive();
}
