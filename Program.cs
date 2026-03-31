using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using uga_mpl_server;
using uga_mpl_server.RequestHelpers;

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

// Connect db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONN_STRING")
    ));

builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

/** Not sure how I would integrate this one as well Alongside Google SSO. Also connect the DB from here. Example shown below.

builder.Services.AddAuthentication(x => 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x=> {
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    
});

builder.Services.AddDbContext<UserDbContext>(opt =>
{
    // opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    opt.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRESQL_CONN_STRING"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

*/

// For running android emulators on PC (localhost passthrough)
builder.WebHost.UseUrls("http://0.0.0.0:5274");

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
