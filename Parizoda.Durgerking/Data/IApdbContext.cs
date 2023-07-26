using Microsoft.EntityFrameworkCore;
using parizoda.durgerking.Entity;

namespace parizoda.durgerking.Data;

public interface IAppDbContext
{
    DbSet<User> Users {get;set;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}