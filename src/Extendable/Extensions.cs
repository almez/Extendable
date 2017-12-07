using Extendable.Abstraction;

namespace Extendable
{
    public static class Extensions
    {
        public static void SetAttribute<TValue>(this IExtendable extendable, string fieldName, TValue fieldValue, TValue defaultValue = default(TValue))
        {

        }

        public static TValue GetAttribute<TValue>(this IExtendable extendable, string fieldName, TValue defaultValue = default(TValue))
        {
            return defaultValue;
        }
    }
}