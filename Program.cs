using uga_mpl_server;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// setting Development ENV variables

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    var cwd = Directory.GetCurrentDirectory();
    DotEnv.Load(Path.Combine(cwd, ".env"));
}

builder.Services.AddControllers();


var app = builder.Build();


// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
