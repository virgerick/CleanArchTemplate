namespace Trasnport.Shared.Extensions;
using System.Text.Json;

public static class StringExtension
{
    public static T ToDeserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json)!;
    }
}