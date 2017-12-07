using Extendable.Abstraction;

namespace Extendable
{
    public static class Extensions
    {
        public static void SetAttribute<T>(this IExtendable extendable, string fieldName, T fieldValue, T defaultValue = default(T))
        {

        }

        public static T GetAttribute<T>(this IExtendable extendable, string fieldName, T defaultValue = default(T))
        {
            return defaultValue;
        }
    }
}