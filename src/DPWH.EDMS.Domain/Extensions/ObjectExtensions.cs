using System.Reflection;

namespace DPWH.EDMS.Domain.Extensions;

public static class ObjectExtensions
{
    public static bool HasAttribute<T>(this object obj) where T : Attribute => obj.GetType().GetTypeInfo().GetCustomAttribute<T>() != null;

    public static T If<T>(this T obj, bool predicate, Action<T> configureAction)
    {
        if (predicate) configureAction(obj);
        return obj;
    }

    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                .GetProperty(item.Key)
                ?.SetValue(someObject, item.Value, null);
        }

        return someObject;
    }
}
