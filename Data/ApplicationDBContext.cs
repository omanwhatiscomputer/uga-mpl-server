using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Entities;
using uga_mpl_server.DTO.User;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}


// public class UserDbContext(DbContextOptions options) : DbContext(options)
// {
//     public DbSet<User> Users { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<User>()
//             .HasIndex(u => u.Email)
//             .IsUnique();

//         modelBuilder.Entity<User>()
//             .HasIndex(u => u.Id)
//             .IsUnique();
//     }

// }