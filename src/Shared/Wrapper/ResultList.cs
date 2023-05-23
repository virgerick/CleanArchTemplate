using CleanArchTemplate.Shared.Extensions;

namespace CleanArchTemplate.Shared.Wrapper;

public class ResultList<T> : Result, IResultList<T>
{
    public List<T> Items { get; set; } = default!;
    public ResultList() : base(){}

    public new static ResultList<T> Failure()
    {
        return new ResultList<T> { Succeeded = false };
    }

    public new static ResultList<T> Failure(string message)
    {
        return new ResultList<T> { Succeeded = false, Messages = new List<string> { message } };
    }

    public new static ResultList<T> Failure(List<string> messages)
    {
        return new ResultList<T> { Succeeded = false, Messages = messages };
    }

    public new static Task<ResultList<T>> FailureAsync()
    {
        return Task.FromResult(Failure());
    }

    public new static Task<ResultList<T>> FailureAsync(string message)
    {
        return Task.FromResult(Failure(message));
    }

    public new static Task<ResultList<T>> FailureAsync(List<string> messages)
    {
        return Task.FromResult(Failure(messages));
    }

    public new static ResultList<T> Success()
    {
        return new ResultList<T> { Succeeded = true };
    }

    public new static ResultList<T> Success(string message)
    {
        return new ResultList<T> { Succeeded = true, Messages = new List<string> { message } };
    }

    public static ResultList<T> Success(List<T> items)
    {
        return new ResultList<T> { Succeeded = true, Items = items };
    }

    public static ResultList<T> Success(List<T> items, string message)
    {
        return new ResultList<T> { Succeeded = true, Items = items, Messages = new List<string> { message } };
    }

    public static ResultList<T> Success(List<T> items, List<string> messages)
    {
        return new ResultList<T> { Succeeded = true, Items = items, Messages = messages };
    }

    public new static Task<ResultList<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<ResultList<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<ResultList<T>> SuccessAsync(List<T> items)
    {
        return Task.FromResult(Success(items));
    }

    public static Task<ResultList<T>> SuccessAsync(List<T> items, string message)
    {
        
        return Task.FromResult(Success(items, message));
    }
    public static async Task<ResultList<T>> TryCatch(Func<Task<ResultList<T>>> action,Action onFinally=default!)
    {
        try
        {
            return await action?.Invoke()!;

        }
        catch (Exception ex)
        {
            return ResultList<T>.Failure(ex.GetMessages().ToList());
        }
        finally
        {
            onFinally?.Invoke();
        }
    }
    public static  ResultList<T> TryCatch(Func<ResultList<T>> action,Action onFinally=default!)
    {
        try
        {
            return  action.Invoke();

        }
        catch (Exception ex)
        {
            return ResultList<T>.Failure(ex.GetMessages().ToList());
        }
        finally
        {
            onFinally?.Invoke();
        }
    }
}