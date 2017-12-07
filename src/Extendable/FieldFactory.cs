using System;
using Extendable.Domain;

namespace Extendable
{
    internal class FieldFactory
    {
        internal static Field CreateField(string holderType, string holderId, string fieldName, string fieldValue, string language)
        {
            var field = new Field()
            {
                HolderType = holderType,
                HolderId = holderId,
                FieldName = fieldName,
                FieldValue = fieldValue,
                Language = language,
            };

            var utcNow = DateTime.UtcNow;
            field.CreatedUtc = utcNow;
            field.LastUpdatedUtc = utcNow;

            return field;
        }
    }
}