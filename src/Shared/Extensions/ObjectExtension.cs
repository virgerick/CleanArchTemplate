using System.Text.Json;

namespace CleanArchTemplate.Shared.Extensions;
public static class ObjectExtension
{
    public static string ToJsonSerialize(this Object obj)
    {
        if (obj is null) return "{}";
        return JsonSerializer.Serialize(obj);
    }

    public static T ToJsonDeserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json)!;
    }
}

public  struct TaskSync
{
    public static async void AwaitSync<T>(Func<Task<T>> func,Action<T> onSuccess, Action<Exception> onError = default!)
    {
        try
        {
            var result=await func.Invoke();
            onSuccess.Invoke(result);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }
    }
    public static async void AwaitSync(Func<Task> func, Action<Exception> onError = default!)
    {
        try
        {
            await func.Invoke();
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }
    }
}

   

