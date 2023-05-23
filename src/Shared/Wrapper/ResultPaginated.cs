using System;
using System.Collections.Generic;
using CleanArchTemplate.Shared.Extensions;

namespace CleanArchTemplate.Shared.Wrapper;

public class ResultPaginated<T> : Result,IResultPaginated<T>
{
    public ResultPaginated(List<T> items)
    {
        Items = items;
    }

    public List<T> Items { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    internal ResultPaginated(bool succeeded, List<T> items = default!, List<string> messages = null!, int count = 0, int page = 1, int pageSize = 10)
    {
        Items = items;
        CurrentPage = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public new static ResultPaginated<T> Failure(List<string> messages)
    {
        return new ResultPaginated<T>(false, default!, messages);
    }

    public static ResultPaginated<T> Success(List<T> items, int count, int page, int pageSize)
    {
        return new ResultPaginated<T>(true, items, null!, count, page, pageSize);
    }


    public static async Task<ResultPaginated<T>> TryCatch(
        Func<Task<ResultPaginated<T>>> action,
        Action onFinally = default!)
    {
        try
        {
            return await action?.Invoke()!;

        }
        catch (Exception ex)
        {
            return ResultPaginated<T>.Failure(ex.GetMessages().ToList());
        }
        finally
        {
            onFinally?.Invoke();
        }
    }
    public static  ResultPaginated<T> TryCatch(
        Func<ResultPaginated<T>> action,
        Action onFinally = default!)
    {
        try
        {
            return  action.Invoke()!;

        }
        catch (Exception ex)
        {
            return ResultPaginated<T>.Failure(ex.GetMessages().ToList());
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

}
