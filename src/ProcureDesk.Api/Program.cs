using Scalar.AspNetCore;
using ProcureDesk.Domain;
using ProcureDesk.Application;
using ProcureDesk.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddSingleton<ISupplierRepository, MockSupplierRepository>();
builder.Services.AddSingleton<IPurchaseOrderRepository, MockPurchaseOrderRepository>();
builder.Services.AddSingleton<ISupplierProductRepository, MockSupplierProductRepository>();

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
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProcureDesk API v1");
        c.RoutePrefix = "swagger";
    });

    // Scalar UI
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/v1.json";
        options.Title = "ProcureDesk API";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
