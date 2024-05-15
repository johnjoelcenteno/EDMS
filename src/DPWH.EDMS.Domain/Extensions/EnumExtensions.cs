using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DPWH.EDMS.Domain.Extensions;

public static class EnumExtensions
{
    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
            typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
        // Or return default(T);
    }

    public static string GetDescriptionFromValue<T>(string value) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute &&
                (field.Name == value || attribute.Description == value))
            {
                return attribute.Description;
            }
        }
        return value;
    }
}