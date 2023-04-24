namespace CleanArchTemplate.Shared.Extensions;
using System.Text.Json;
public static class ObjectExtension
{
    public static string ToJsonSerialize(this Object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
