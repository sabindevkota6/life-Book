using Microsoft.EntityFrameworkCore;
using life_Book.Data.Models;

namespace life_Book.Data.Utils;

public class DatabaseSchema : DbContext
{
    public DbSet<User> Users { get; set; }

    public DatabaseSchema(DbContextOptions<DatabaseSchema> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

        // User table configuration
        modelBuilder.Entity<User>(entity =>
   {
   entity.HasKey(e => e.Id);
   entity.Property(e => e.Username).IsRequired();
         entity.Property(e => e.Email).IsRequired();
       entity.HasIndex(e => e.Email).IsUnique();
         entity.HasIndex(e => e.Username).IsUnique();
   });
    }
}
