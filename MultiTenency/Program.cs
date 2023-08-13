using Microsoft.EntityFrameworkCore;
using MultiTenency.Data;
using MultiTenency.Services;
using MultiTenency.Setings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TenantSetting>(builder.Configuration.GetSection(nameof(TenantSetting)));
TenantSetting options = new();
builder.Configuration.GetSection(nameof(TenantSetting)).Bind(options);

#region Registration Of Service 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITenantServices, TenantServices>();
builder.Services.AddScoped<IProductService, ProductService>();

#endregion

#region Database Provider
var defaultDbProvider = options.Defaults.DBProvider;
if (defaultDbProvider.ToLower() == "mssql")
{
    builder.Services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
}
foreach (var tenant in options.Tenants)
{
    var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.SetConnectionString(connectionString);
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
