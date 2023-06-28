namespace CleanArchTemplate.Shared.Results;

public static class ResultExtensions
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn,TOut> mappingFunction)
    {
        return result.Succeeded ? Result.Success(mappingFunction(result.Value)) :
            Result.Failure<TOut>(result.Errors);
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> mappingFunction)
    {
        return result.Succeeded ? await mappingFunction(result.Value): Result.Failure<TOut>(result.Errors); ;
    }
    public static Result Bind<TIn, TOut>(this Result<TIn> result, Func<TIn,Result<TOut>> bindFunction) => 
        result.Succeeded ? bindFunction(result.Value) : Result.Failure<TOut>(result.Errors);
    
    public static Result Bind<TIn>(this Result<TIn> result, Func<TIn, Result> bindFunction)
        => result.IsFailure ? Result.Failure(result.Errors) : bindFunction(result.Value);

    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> bindFunction)
        => result.IsFailure?Result.Failure(result.Errors): await bindFunction(result.Value);
    
    public static async Task<Result> Bind<TIn,TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bindFunc)
        => result.IsFailure?Result.Failure(result.Errors):await bindFunc(result.Value);

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if(result.Succeeded) action(result.Value);
        return result;
        
    }
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<TIn, Task> action)
    {
        if(result.Succeeded) await action(result.Value);
        return result;
    }
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<Task> action)
    {
        if(result.Succeeded) await action();
        return result;
    }
}
public static class ExceptionExtensions{

    public static IEnumerable<Error> ToErrors(this Exception exception)
    {
        var errors = new List<Results.Error>();
        var current = exception;
        while (current!= null)
        {
            errors.Add(Error.Create(current.Source!, current.Message));
            current = current.InnerException;
        }
        return errors.ToArray();
    }
    public static IEnumerable<Error> ToError(this Exception exception)
    {
        var errors = new List<Results.Error>();
        var current = exception;
        while (current!= null)
        {
            errors.Add(Error.Create(current.Source!, current.Message));
            current = current.InnerException;
        }
        return errors.ToArray();
    }
}