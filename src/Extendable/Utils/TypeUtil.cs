using System;
using System.Globalization;

namespace Extendable.Utils
{
    public class TypeUtil
    {
        public static int IntSize => sizeof(int);

        public static int StringSize(string str) => (str?.Length ?? 0) * sizeof(char);

        public static T ChangeType<T>(object value, T defaultValue = default(T))
        {
            try
            {
                var t = typeof(T);

                if (t.IsEnum)
                {
                    return (T)Enum.Parse(t.UnderlyingSystemType, value.ToString());
                }

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (value == null)
                    {
                        return default(T);
                    }

                    t = Nullable.GetUnderlyingType(t);
                }

                return (T)Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
