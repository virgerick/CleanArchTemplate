using System.Runtime.InteropServices.JavaScript;

namespace CleanArchTemplate.Shared.Results;

public class Result
{
    protected  Result(bool succeeded, Error error)
    {
        Succeeded = succeeded;
        Errors = new[]{error};
    }
    protected Result(bool succeeded, IEnumerable<Error> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }
    public bool Succeeded { get; }
    public bool IsFailure => !Succeeded;
    public IEnumerable<Error> Errors { get; }
    public static Result Success()=>new(true,Error.None);
    public static Result<TValue> Success<TValue>(TValue value)=>new Result<TValue>(value,true,Error.None);
    public static Result Failure(Error error) => new(false,error);
    public static Result Failure(IEnumerable<Error> errors) =>  new(false,errors);
    public static Result<TValue> Failure<TValue>( Error error) => new Result<TValue>(default!,false,error);
    public static Result<TValue> Failure<TValue>(IEnumerable<Error> errors) => new Result<TValue>(default!,false,errors);

    public static Result<TValue> Create<TValue>(TValue value, Error error)=>value is not null ?   Success<TValue>(value) :  Failure<TValue>(error);
    
    public static Result<T> Ensure<T>(T value, params (Func<T, bool> predicate, Error error)[] functions)
    {
        var results=new List<Result<T>>();
        foreach (var ( predicate,  error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }
        return Combine<T>(results.ToArray());
    }
    public static Result<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        if (predicate(value)) return Success<T>(value);
        return Failure<T>(error);
    }
    public  static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            var errors = GetDistinctErrors(results);
            return Failure<T>(errors);
        }
        return Success(results[0].Value);
    }

    public static IEnumerable<Error> GetDistinctErrors<T>(Result<T>[] results)
    {
        return results.SelectMany(e => e.Errors)
            .Where(e => e != Error.None)
            .Distinct();
    }

    public static Result<(T1, T2)> Combine<T1, T2>(Result<T1> result1, Result<T2> result2)=> (result1.IsFailure)? 
            Failure<(T1,T2)>(result1.Errors):
            result2.IsFailure ? 
            Failure<(T1,T2)>(result2.Errors) : 
            Success((result1.Value, result2.Value));

    public static Result TryCatch(Func<Result> action)
    {
        try
        {
            return action();
        }
        catch (Exception e)
        {
            return e;
        }
    }   
    public static async Task<Result> TryCatch(Func<Task<Result>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            return e;
        }
    }
    public static Result<T> TryCatch<T>(Func<Result<T>> action)
    {
        try
        {
            return  action();
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }   
    public static async Task<Result<T>> TryCatch<T>(Func<Task<Result<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
    
    public static implicit operator Result(Exception exception) => Failure(exception);

}

public class Result<T> : Result
{
    public T Value { get; }
    protected internal Result(T value, bool succeeded, Error error) : base(succeeded, error)
    {
        Value = value;
    }
    protected internal Result(T value, bool succeeded, IEnumerable<Error> errors) : base(succeeded, errors)
    {
        Value = value;
    }
    public static implicit operator Result<T>(T value) => Create(value,Error.Create("NullValue","Value is null"));
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
    public static implicit operator Result<T>(Error[] errors) => Failure<T>(errors);
}