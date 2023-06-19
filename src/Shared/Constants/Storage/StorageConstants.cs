using System.Reflection;

namespace CleanArchTemplate.Shared.Constants.Storage;

public static class StorageConstants
{
    public  class Local: StorageKeys<Local>
    {
        public static string Preference = "3abeb8ae-0e0d-49e2-a024-950833da5ed5";
        public static string LoggedUserData = "83243f7e-aec2-47b3-86d1-936106d3234f";
        public static string AuthToken = "295030fc-8f19-45e6-9d81-d7218ae754ce";
        public static string RefreshToken = "85a6eaee-6f76-43f2-b24f-79dedf62da58";
        public static string UserImageURL = "2781de17-7f82-4a71-9c89-6b4968a1dae6";
    }
    public  class Server: StorageKeys<Server>
    {
        public static string Preference = "d823d9af-f558-46e0-8316-cdea709a49a7";
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