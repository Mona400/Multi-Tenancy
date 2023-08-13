using Microsoft.Extensions.Options;
using MultiTenency.Setings;

namespace MultiTenency.Services
{
    public class TenantServices : ITenantServices
    {
        private HttpContext? _httpContext;
        private readonly TenantSetting _tenantSettings;
        private Tenants? _currentTenant;
        public TenantServices(IHttpContextAccessor httpContextAccessor, IOptions<TenantSetting> tenantSetting)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _tenantSettings = tenantSetting.Value;
            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetCurrentTenant(tenantId!);
                }

                else
                {
                    throw new Exception("No Tenant Provided!");
                }

            }

        }
        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null ? _tenantSettings.Defaults.ConnectionString : _currentTenant.ConnectionString;
            return currentConnectionString;
        }

        public Tenants? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }

        private void SetCurrentTenant(string? tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(x => x.TId == tenantId);
            if (_currentTenant == null)
            {
                throw new Exception($"Invalid Tenant {tenantId}");
            }
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
