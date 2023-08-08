
using CleanArchTemplate.Shared.Extensions;

namespace CleanArchTemplate.Shared.Wrapper;

public class Result : IResult
{
   

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
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; init; }=default!;

    public new static Result<T> Failure()
    {
        return new Result<T> { Succeeded = false };
    }

    public new static Result<T> Failure(string message)
    {
        return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
    }

    public  static Result<T> Failure(List<string> messages)
    {
        return new Result<T> { Succeeded = false, Messages = messages };
    }

    public new static Task<Result<T>> FailureAsync()
    {
        return Task.FromResult(Failure());
    }

    public  static Task<Result<T>> FailureAsync(string message)
    {
        return Task.FromResult(Failure(message));
    }

    public  static Task<Result<T>> FailureAsync(IEnumerable<string> messages)
    {
        return Task.FromResult(Failure<T>(messages));
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

    public static async Task<Result<T>> TryCatch(Func<Task<Result<T>>> action, Action? onFinally = null)
    {
        try
        {
            var result= await action.Invoke();
            return result;
        }
        catch (Exception ex)
        {
            return Failure(ex.GetMessages().ToList());
        }
        finally
        {
            
            onFinally?.Invoke();
        }
    }
    public static implicit operator Result<T>(T data) => Success<T>(data);
    public static implicit operator Result<T>(Exception error) => Failure<T>(error.GetMessages());
}
public class Result<TSuccess,TError> 
where TSuccess : class 
where TError :Exception
{
    private readonly TSuccess _data = default!;
    private readonly TError _error = default!;
    private readonly bool _succeeded;
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