using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellation);
    Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken = default);

}

