using Microsoft.EntityFrameworkCore;
using MultiTenency.Models;
using MultiTenency.Services;

namespace MultiTenency.Data
{
    public class ApplicationDbContext : DbContext

    {
        public string TenantId { get; set; }
        private readonly ITenantServices _tenantServices;
        public ApplicationDbContext(DbContextOptions options, ITenantServices tenantServices) : base(options)
        {
            _tenantServices = tenantServices;
            TenantId = _tenantServices.GetCurrentTenant()?.TId;
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //global filter to entity
            modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenentId == TenantId);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                entity.Entity.TenentId = TenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantServices.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantServices.GetDatabaseProvider();
                if (dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }
    }
}
