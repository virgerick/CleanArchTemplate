namespace CleanArchTemplate.Client.Extensions;

public static class EnumExtensions
{
    public static T GetRandomValue<T>(this T value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return  values.GetRandomElement();
    }
}
