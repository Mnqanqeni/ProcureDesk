using Scalar.AspNetCore;
using ProcureDesk.Application;
using ProcureDesk.Domain.Abstractions;
using ProcureDesk.Domain;
using ProcureDesk.Infrastructure;
using ProcureDesk.Infrastructure.Security;
using ProcureDesk.Infrastructure.Repositories;
using dotenv.net;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load(options: new DotEnvOptions(
    probeForEnv: true,
    probeLevelsToSearch: 5
));

var builder = WebApplication.CreateBuilder(args);

var baseConn = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(baseConn))
{
    // Startup failure is correct here: app cannot function without DB.
    throw new InvalidOperationException("Missing ConnectionStrings:Default in appsettings.json (or other config).");
}

var saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");
if (string.IsNullOrWhiteSpace(saPassword))
{
    throw new InvalidOperationException("SA_PASSWORD env var not set.");
}

// Build final connection string once, store it back into config
builder.Configuration["ConnectionStrings:Default"] = $"{baseConn};Password={saPassword}";

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register repositories
// NOTE: If you register both Mock + Real, the LAST one wins for single injection.
// Pick one approach per interface, or inject IEnumerable<T> if you want both.

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ISupplierRepository, MockSupplierRepository>();
builder.Services.AddSingleton<IPurchaseOrderRepository, MockPurchaseOrderRepository>();
builder.Services.AddSingleton<ISupplierProductRepository, MockSupplierProductRepository>();

builder.Services.AddSingleton<DbConnectionFactory>();

// ✅ You will need a real IUserRepository for auth to work.
// If you already have UserRepository, register it here:
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register application services
builder.Services.AddScoped<UserApplicationService>();
builder.Services.AddScoped<ProductApplicationService>();
builder.Services.AddScoped<SuppliersApplicationService>();
builder.Services.AddScoped<SupplierProductApplicationService>();


// Register security adapters
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();


var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Missing configuration: Jwt:Secret");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
            ValidIssuer = jwtIssuer,

            ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// ✅ These two lines make [Authorize] work
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();