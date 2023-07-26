using Microsoft.EntityFrameworkCore;
using Sirojiddin.Durgerking.Entity;

namespace Sirojiddin.Durgerking.Data;
public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    Task <int> SaveChangesAsync(CancellationToken cancellationToken = default);
}