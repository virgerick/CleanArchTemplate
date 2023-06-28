using System.Text.Json;

namespace CleanArchTemplate.Shared.Extensions;
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



