using System;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CleanArchTemplate.Client.Extensions;

public static class ComponentExtension
{
	
}


public static class ArrayExtensions
{
    private static readonly Random Random = new Random();

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            throw new ArgumentException("El array no puede ser null y debe tener al menos un elemento.");
        }

        var index = Random.Next(array.Length);
        return array[index];
    }
}
public static class EnumExtensions
{
    public static T GetRandomValue<T>(this T value) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return  values.GetRandomElement();
    }
}