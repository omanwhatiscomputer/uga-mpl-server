using uga_mpl_server;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// setting Development ENV variables

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    var cwd = Directory.GetCurrentDirectory();
    DotEnv.Load(Path.Combine(cwd, ".env"));
}

builder.Services.AddControllers();

/**
builder.Services.AddDbContext<UserDbContext>(opt =>
{
    opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONN_STRING"));
});
*/


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




// app.UseHttpsRedirection();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com",
            ValidateAudience = true,
            // whitelist all three IDs --> backend recognizes tokens from any source
            ValidAudiences = new[]
            {
                Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_ANDROID"),
                Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_IOS"),
                Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENT_ID_WEB")
            },
            ValidateLifetime = true
        };
    });


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
