namespace MultiTenency.Setings
{
    public class TenantSetting
    {
        public Configration Defaults { get; set; } = default!;
        public List<Tenants> Tenants { get; set; } = new();
    }
}
