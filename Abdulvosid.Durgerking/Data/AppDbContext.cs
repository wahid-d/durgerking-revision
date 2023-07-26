using Abdulvosid.Durgerking.Entity;
using Microsoft.EntityFrameworkCore;

namespace Abdulvosid.Durgerking.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Fullname)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasMaxLength(30);
        modelBuilder.Entity<User>()
            .Property(u => u.Language)
            .HasMaxLength(10);
        modelBuilder.Entity<User>()
            .Property(u => u.Phone)
            .HasMaxLength(12);
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));
        modelBuilder.Entity<User>()
            .Property(u => u.ModifiedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));

        base.OnModelCreating(modelBuilder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        => base.SaveChangesAsync(cancellation);
}