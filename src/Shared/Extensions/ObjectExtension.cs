using System.Text.Json;

namespace Trasnport.Shared.Extensions;
public static class ObjectExtension
{
    public static string ToJsonSerialize(this object obj)
    {
        if (obj != null!) return JsonSerializer.Serialize(obj);
        return "{}";
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

   

