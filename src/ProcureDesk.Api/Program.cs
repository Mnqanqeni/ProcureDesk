using Scalar.AspNetCore;
using ProcureDesk.Domain;
using ProcureDesk.Application;
using ProcureDesk.Infrastructure;
using ProcureDesk.Domain.Abstractions;
using dotenv.net;

DotEnv.Load(options: new DotEnvOptions(
    probeForEnv: true, 
    probeLevelsToSearch: 5
));

var builder = WebApplication.CreateBuilder(args);

var baseConn = builder.Configuration.GetConnectionString("Default")
              ?? builder.Configuration["ConnectionStrings:Default"]
              ?? builder.Configuration["DefaultConnectionString"];

if (string.IsNullOrWhiteSpace(baseConn))
    throw new Exception("Missing ConnectionStrings:Default in appsettings.json (or other config).");

var saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD")
                 ?? throw new Exception("SA_PASSWORD env var not set.");

builder.Configuration["ConnectionStrings:Default"] = $"{baseConn};Password={saPassword}";

var connString = $"{baseConn};Password={saPassword}";

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register repositories
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ISupplierRepository, MockSupplierRepository>();
builder.Services.AddSingleton<IPurchaseOrderRepository, MockPurchaseOrderRepository>();
builder.Services.AddSingleton<ISupplierProductRepository, MockSupplierProductRepository>();
builder.Services.AddSingleton<DbConnectionFactory>();

// Register application services
builder.Services.AddScoped<ProductApplicationService>();
builder.Services.AddScoped<SuppliersApplicationService>();
// builder.Services.AddScoped<PurchaseOrdersApplicationService>();
builder.Services.AddScoped<SupplierProductApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
