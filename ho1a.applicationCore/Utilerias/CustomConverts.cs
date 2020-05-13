using System;

namespace ho1a.applicationCore.Utilerias
{
    public class CustomConverts
    {
        public static int? ConvertStringToInt(string value, bool? canNull = null)
        {
            if (string.IsNullOrEmpty(value)) return canNull != null && canNull.Value ? (int?)null : 0;

            return Convert.ToInt32(value);
        }
    }
}
