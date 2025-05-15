using AuthApi.Services;
using AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger death
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Retrieve Configurations
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Ensure Configurations is set in production
builder.Services.PostConfigure<JwtSettings>(settings =>
{
    if (string.IsNullOrWhiteSpace(settings.SecretKey))
        throw new Exception("JWT SecretKey is not set");
});

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.MapControllers();
app.MapOpenApi();

app.Run();