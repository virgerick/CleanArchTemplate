using CleanArchTemplate.Application.Common.Exceptions;
using CleanArchTemplate.Application.Common.Specifications.Base;
using CleanArchTemplate.Domain;
using CleanArchTemplate.Shared.Wrapper;

using Microsoft.EntityFrameworkCore;

namespace CleanArchTemplate.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<ResultPaginated<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            if (source == null) throw new ApiException("Source has not elements.");
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return ResultPaginated<T>.Success(items, count, pageNumber, pageSize);
        }
        public static  Task<ResultList<T>> ToResultListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken=default) where T : class
        {
            return ResultList<T>.TryCatch(async () =>
            {
                if (source == null) throw new ApiException("Source has not elements.");
                List<T> items = await source.ToListAsync(cancellationToken);
                return ResultList<T>.Success(items);
            });
        }

        public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(query,
                    (current, include) => current.Include(include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return secondaryResult.Where(spec.Criteria);
        }
    }
}