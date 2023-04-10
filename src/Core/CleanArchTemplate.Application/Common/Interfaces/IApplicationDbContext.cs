using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CleanArchTemplate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellation);
    Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken=default);
   
}

