using Microsoft.EntityFrameworkCore;
using uga_mpl_server.Entities;

namespace uga_mpl_server.Data;

public class ApplicationDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();
    }
}
