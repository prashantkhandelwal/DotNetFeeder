using Newtonsoft.Json;

public static class Utils
{
    public static string ToJSON<T>(this T Entity) where T : class
    {
        return JsonConvert.SerializeObject(Entity);
    }

    public static T ToObject<T>(this string Entity) where T : class
    {
        return JsonConvert.DeserializeObject<T>(Entity);
    }
}