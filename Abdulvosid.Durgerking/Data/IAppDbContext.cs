using Abdulvosid.Durgerking.Entity;
using Microsoft.EntityFrameworkCore;

namespace Abdulvosid.Durgerking.Data;

public interface IAppDbContext 
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}