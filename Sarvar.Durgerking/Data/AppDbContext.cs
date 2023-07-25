using Microsoft.EntityFrameworkCore;
using Sarvar.Durgerking.Entity;

namespace Sarvar.Durgerking.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(a => a.Id);
        
        modelBuilder.Entity<User>()
            .Property(a => a.Fullname)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<User>()
            .Property(a => a.Username)
            .HasMaxLength(50);
        
        modelBuilder.Entity<User>()
            .Property(a => a.Language)
            .HasMaxLength(10);
        
        modelBuilder.Entity<User>()
            .Property(a => a.Phone)
            .HasMaxLength(20);
        
        modelBuilder.Entity<User>()
            .Property(a => a.CreatedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));
        
        modelBuilder.Entity<User>()
            .Property(a => a.ModifiedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}