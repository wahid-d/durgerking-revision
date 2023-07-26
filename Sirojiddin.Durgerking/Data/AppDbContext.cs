using Microsoft.EntityFrameworkCore;
using Sirojiddin.Durgerking.Data;
using Sirojiddin.Durgerking.Entity;

public class AppDbContext : DbContext,IAppDbContext
{
    public DbSet<User>  Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected  override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<User>()
            .Property(e => e.Fullname)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(e => e.Username)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(e => e.Language)
            .HasMaxLength(10);

        modelBuilder.Entity<User>()
            .Property(e => e.Phone)
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(e => e.CreatedAt);
            
        modelBuilder.Entity<User>()
            .Property(e => e.ModifiedAt);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}