using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CodeSamples.ScrappedItems.Utilities
{
    public static class EnumUtilities
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetDescription(this Enum enumerationValue)
        {
            var value = enumerationValue.GetEnumAttributeValue(typeof(DescriptionAttribute));
            if (value != null)
            {
                return ((DescriptionAttribute)value).Description;
            }
            return enumerationValue.ToString();
        }

        public static object GetEnumAttributeValue<T>(this T enumerationValue, Type attributeType)
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo?.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(attributeType, false);
                if (attrs?.Length > 0)
                {
                    return attrs[0];
                }
            }
            return null;
        }
    }
}