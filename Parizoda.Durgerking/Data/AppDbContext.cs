using Microsoft.EntityFrameworkCore;
using parizoda.durgerking.Entity;

namespace parizoda.durgerking.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){ }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasKey(p => p.Id);

        builder.Entity<User>()
            .Property(p => p.Fullname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Entity<User>()
            .Property(p => p.Username)
            .HasMaxLength(50);

        builder.Entity<User>()
            .Property(p => p.Language)
            .HasMaxLength(15);

        builder.Entity<User>()
            .Property(p => p.Phone)
            .HasMaxLength(15);

        builder.Entity<User>()
            .Property(p => p.CreatedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));

        builder.Entity<User>()
            .Property(p => p.ModifiedAt)
            .HasDefaultValue(new DateTime(2023, 7, 10, 11, 29, 16, 314, DateTimeKind.Utc).AddTicks(6142));

        

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}