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

    public static T GetValueFromName<T>(string name) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (string.Equals(field.Name, name, StringComparison.OrdinalIgnoreCase))
                return (T)field.GetValue(null);
        }

        throw new AppException($"Enum not found: '{name}' in {typeof(T).Name}");
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

    public static string GetDescription(this Enum genericEnum)
    {
        var genericEnumType = genericEnum.GetType();
        var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((attribs != null && attribs.Length != 0))
            {
                return ((DescriptionAttribute)attribs.ElementAt(0)).Description;
            }
        }
        return genericEnum.ToString();
    }
}