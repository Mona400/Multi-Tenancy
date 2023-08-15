using MultiTenency.Setings;

namespace MultiTenency.Services
{
    public interface ITenantServices
    {
        string? GetConnectionString();
        Tenants? GetCurrentTenant();
        string? GetDatabaseProvider();
    }
}
