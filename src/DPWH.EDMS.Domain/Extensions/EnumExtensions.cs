using DPWH.EDMS.Domain.Exceptions;
using System.ComponentModel;

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

        throw new AppException($"Enum not found: '{description}' in {typeof(T).Name}" );        
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