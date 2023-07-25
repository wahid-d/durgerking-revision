using Microsoft.EntityFrameworkCore;
using Sarvar.Durgerking.Entity;

namespace Sarvar.Durgerking.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}