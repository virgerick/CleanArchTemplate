using System.Reflection;

namespace CleanArchTemplate.Shared.Constants.Storage;

public static class StorageConstants
{
    public  class Local: StorageKeys<Local>
    {
        public static string Preference = "58>Mv/[q*bw,";
        public static string LoggedUserData = "by3+Cdg*e*";
        public static string AuthToken = "6*qJjaJsTG";
        public static string RefreshToken = ".a{qW)tn+W";
        public static string UserImageURL = "s{My5/$K;P";
    }
    public  class Server: StorageKeys<Server>
    {
        public static string Preference = "e2d}^CCQ29";
    }
}
public class StorageKeys<T>
{
    public static IEnumerable<string> Keys
    {
        get
        {
            var type = typeof(T);
            foreach(var prop in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    yield return propertyValue.ToString()!;
            }

            foreach (var prop in type.GetNestedTypes()
                .SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    yield return propertyValue.ToString()!;
            }
        }
    }
}