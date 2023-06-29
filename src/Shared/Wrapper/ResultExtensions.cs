namespace CleanArchTemplate.Shared.Wrapper;

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