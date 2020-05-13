using System;
using System.ComponentModel;
using System.Globalization;

namespace ho1a.applicationCore.Utilerias
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                var type = e.GetType();
                var values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }
            return description;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            T result = default(T);
            var type = typeof(T);
            var values = Enum.GetValues(type);

            foreach (var val in values)
            {
                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionAttributes.Length > 0)
                {
                    if (((DescriptionAttribute)descriptionAttributes[0]).Description.Equals(value))
                    {
                        Enum.TryParse(val.ToString(), out result);
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
