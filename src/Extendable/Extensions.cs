using System;
using Extendable.Abstraction;

namespace Extendable
{
    public static class Extensions
    {
        public static void SetAttribute<TValue>(this IExtendable extendable, string fieldName, TValue fieldValue, string language = "en")
        {
            if (!Configuration.IsFieldProviderRegistered)
            {
                throw new Exception("Field Provider has not been registered yet");
            }

            var holderType = extendable.GetType().Name;
            var holderId = extendable.Id.ToString();

            Configuration.FieldProvider.AddOrUpdateField(holderType, holderId, fieldName, fieldValue, language);
        }

        public static TValue GetAttribute<TValue>(this IExtendable extendable, string fieldName, TValue defaultValue = default(TValue), string language = "en")
        {
            if (!Configuration.IsFieldProviderRegistered)
            {
                throw new Exception("Field Provider has not been registered yet");
            }

            var holderType = extendable.GetType().Name;
            var holderId = extendable.Id.ToString();

            return Configuration.FieldProvider.GetFieldValue<TValue>(holderType, holderId, fieldName, defaultValue, language);
        }
    }
}