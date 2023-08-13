namespace MultiTenency.Setings
{
    public class Tenants
    {
        public string Name { get; set; } = null!;
        public string TId { get; set; } = null!;
        public string? ConnectionString { get; set; }
    }
}
