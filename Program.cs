using uga_mpl_server;
using uga_mpl_server.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    var cwd = Directory.GetCurrentDirectory();
    DotEnv.Load(Path.Combine(cwd, ".env"));
}

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRESQL_CONN_STRING"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = "AppJwt";
    x.DefaultChallengeScheme = "AppJwt";
    x.DefaultScheme = "AppJwt";
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
{
    x.Authority = "https://accounts.google.com";
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://accounts.google.com",
        ValidateAudience = true,
        ValidAudiences = new[]
        {
            Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_ANDROID"),
            Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_IOS"),
            Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_WEB")
        },
        ValidateLifetime = true
    };
})
.AddJwtBearer("AppJwt", x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// For running android emulators on PC (localhost passthrough)
builder.WebHost.UseUrls("http://0.0.0.0:5274");

var app = builder.Build();

DbInitializer.InitDb(app);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
