using MultiTenency.Setings;

namespace MultiTenency.Services
{
    public interface ITenantServices
    {
        public string? GetConnectionString();
        public Tenants? GetCurrentTenant();
        public string? GetDatabaseProvider();
    }
}
