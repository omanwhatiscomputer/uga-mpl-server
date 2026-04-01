using Microsoft.EntityFrameworkCore;

namespace uga_mpl_server.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        context.Database.Migrate();
    }
}
