
// namespace uga_mpl_server.Data;

// using System;
// using Microsoft.EntityFrameworkCore;


// public class DbInitializer
// {
//     public static void InitDb(WebApplication app)
//     {
//         using var scope = app.Services.CreateScope();

//         var context = scope.ServiceProvider.GetService<DbContext>()
//             ?? throw new InvalidOperationException("Failed to retrieve DbContext from the service provider.");

//         context.Database.Migrate();

//     }
// }