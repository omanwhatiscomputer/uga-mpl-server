

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