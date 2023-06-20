using System.Collections.Generic;
using CleanArchTemplate.Shared.Extensions;

namespace CleanArchTemplate.Shared.Wrapper;

public interface IResult
{
    List<string> Messages { get; set; }
    bool Succeeded { get; set; }
}
public interface IResult<out T> : IResult
{
    T Data { get; }
}
public interface IResultPaginated<T> : IResult
{
    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}
public interface IResultList<T> : IResult
{
    public List<T> Items { get; set; }
}

public static class ResultExtension
{
    public static void ThrowIfNotSucceeded(this IResult result)
    {
        if (!result.Succeeded)
        {
            throw result.Messages.ToException();
        }
    }
}
