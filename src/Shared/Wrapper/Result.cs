using System.Collections.Generic;
using System.Threading.Tasks;
using Trasnport.Shared.Extensions;

namespace CleanArchTemplate.Shared.Wrapper;

public class Result : IResult
{
    public Result()
    {
    }

    public IEnumerable<string> Messages { get; set; } = Enumerable.Empty<string>();

    public bool Succeeded { get; set; }

    public static Result Failure()
    {
        return new Result { Succeeded = false };
    }

    public static Result Failure(string message)
    {
        return new Result { Succeeded = false, Messages = new List<string> { message } };
    }

    public static Result Failure(IEnumerable<string> messages)
    {
        return new Result { Succeeded = false, Messages = messages };
    }
    public static Result<T> Failure<T>(string message)
    {
        return new Result<T> { Succeeded = false, Data = default!,Messages = new List<string> { message } };
    }

    public static Result<T> Failure<T>(IEnumerable<string> messages)
    {
        return new Result<T> { Succeeded = false, Data = default!,Messages =  messages  };
    }

    public static Task<Result> FailureAsync()
    {
        return Task.FromResult(Failure());
    }

    public static Task<Result> FailureAsync(string message)
    {
        return Task.FromResult(Failure(message));
    }

    public static Task<Result> FailureAsync(List<string> messages)
    {
        return Task.FromResult(Failure(messages));
    }

    public static Result Success()
    {
        return new Result { Succeeded = true };
    }public static Result<T> Success<T>(T data)
    {
        return new Result<T> { Data=data, Succeeded = true };
    }

    public static Result Success(string message)
    {
        return new Result { Succeeded = true, Messages = new List<string> { message } };
    }

    public static Task<Result> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<Result> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class Result<T> : Result, IResult<T>
{
    public Result()
    {}

    public T Data { get; set; }

    public new static Result<T> Failure()
    {
        return new Result<T> { Succeeded = false };
    }

    public new static Result<T> Failure(string message)
    {
        return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
    }

    public new static Result<T> Failure(List<string> messages)
    {
        return new Result<T> { Succeeded = false, Messages = messages };
    }

    public new static Task<Result<T>> FailureAsync()
    {
        return Task.FromResult(Failure());
    }

    public new static Task<Result<T>> FailureAsync(string message)
    {
        return Task.FromResult(Failure(message));
    }

    public new static Task<Result<T>> FailureAsync(List<string> messages)
    {
        return Task.FromResult(Failure(messages));
    }

    public new static Result<T> Success()
    {
        return new Result<T> { Succeeded = true };
    }

    public new static Result<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Messages = new List<string> { message } };
    }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { message } };
    }

    public static Result<T> Success(T data, List<string> messages)
    {
        return new Result<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<Result<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<Result<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Result<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }

    public static async Task<Result<T>> TryCatch(Func<Task<Result<T>>> action, Action onFinally = default!)
    {
        try
        {
            return await action?.Invoke()!;

        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.GetMessages().ToList());
        }
        finally
        {
            onFinally?.Invoke();
        }
    }
    public static implicit operator Result<T>(T data) => Success<T>(data);
}
public class Result<TSuccess,TError> 
where TSuccess : class 
where TError :Exception
{
    private readonly TSuccess _data = default!;
    private readonly TError _error = default!;
    private readonly bool _succeeded=false;
    public Result(TSuccess data)
    {   _succeeded = true;
        _data = data;
    }
    public Result(TError error)
    {
        _succeeded = false;
        _error = error;
    }
    public static implicit operator Result<TSuccess, TError>(TSuccess success) => new(success);
    public static implicit operator Result<TSuccess, TError>(TError error) => new(error);
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onError)
     => _succeeded ? onSuccess(_data) : onError(_error);
    public void Switch(Action<TSuccess> onSuccess, Action<TError> onError){
        if(_succeeded){
             onSuccess.Invoke(_data);
            return;
        }
        onError.Invoke(_error);
    } 
    
}

public static class ResultExtensions
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn,TOut> mappingFunction)
    {
        return result.Succeeded ? Result.Success(mappingFunction(result.Data)) :
            Result.Failure<TOut>(result.Messages);
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> mappingFunction)
    {
        return result.Succeeded ? await mappingFunction(result.Data): Result.Failure<TOut>(result.Messages); ;
    }
    public static Result Bind<TIn, TOut>(this Result<TIn> result, Func<TIn,Result<TOut>> bindFunction) => 
        result.Succeeded ? bindFunction(result.Data) : Result.Failure(result.Messages);
    
    public static Result Bind<TIn>(this Result<TIn> result, Func<TIn, Result> bindFunction)
        => !result.Succeeded ? Result.Failure(result.Messages) : bindFunction(result.Data);

    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> bindFunction)
        => !result.Succeeded?Result.Failure(result.Messages): await bindFunction(result.Data);
    
    public static async Task<Result> Bind<TIn,TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bindFunc)
        => !result.Succeeded?Result.Failure(result.Messages):await bindFunc(result.Data);

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if(result.Succeeded) action(result.Data);
        return result;
        
    }
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<TIn, Task> action)
    {
        if(result.Succeeded) await action(result.Data);
        return result;
    }
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<Task> action)
    {
        if(result.Succeeded) await action();
        return result;
    }
}