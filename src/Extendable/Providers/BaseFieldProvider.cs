using System;
using CachingManager;
using Extendable.Abstraction;
using Extendable.Domain;
using Extendable.Utils;

namespace Extendable.Providers
{
    public abstract class BaseFieldProvider : IFieldProvider
    {
        #region Fields

        public Lazy<Cache<Field, string>> FieldCache = new Lazy<Cache<Field, string>>(() => CacheFactory.CreateCache<Field, string>("Field Cache", Configuration.CacheSizeLimit));

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void AddOrUpdateField<TValue>(string holderType, string holderId, string fieldName, TValue fieldValue, string language = "en")
        {
            var serializedFieldValue = fieldValue.ToString();

            if (this.IsExist(holderType, holderId, fieldName, language))
            {
                this.UpdateField(holderType, holderId, fieldName, serializedFieldValue, language);
            }
            else
            {
                this.AddField(holderType, holderId, fieldName, serializedFieldValue, language);
            }
        }

        /// <inheritdoc />
        public bool IsExist(string holderType, string holderId, string fieldName, string language = "en")
        {
            return this.GetField(holderType, holderId, fieldName, language) != null;
        }

        /// <inheritdoc />
        public TValue GetFieldValue<TValue>(string holderType, string holderId, string fieldName, TValue defaultValue = default(TValue), string language = "en")
        {
            var field = GetField(holderType, holderId, fieldName, language);

            if (field == null) return defaultValue;

            var deserializedFieldValue = TypeUtil.ChangeType(field.FieldValue, defaultValue);

            return deserializedFieldValue;
        }

        /// <inheritdoc />
        public void ClearCachedFields()
        {
            if (Configuration.CacheEnabled)
            {
                this.FieldCache.Value.Clear();
            }
        }

        /// <inheritdoc />
        public Field GetField(string holderType, string holderId, string fieldName, string language = "en")
        {
            Field field = null;

            if (Configuration.CacheEnabled)
            {
                var cacheEntryId = CacheEntryIdFormatter(holderType, holderId, fieldName, language);

                field = this.FieldCache.Value[cacheEntryId];
            }

            if (field == null)
            {
                field = GetFieldFromDb(holderType, holderId, fieldName, language);
            }

            return field;
        }

        /// <inheritdoc />
        public void AddField(string holderType, string holderId, string fieldName, string serializedFieldValue, string language = "en")
        {
            var field = FieldFactory.CreateField(holderType, holderId, fieldName, serializedFieldValue, language);

            AddFieldValueToDb(field);

            if (Configuration.CacheEnabled)
            {
                var cacheEntryId = CacheEntryIdFormatter(holderType, holderId, fieldName, language);

                 this.FieldCache.Value[cacheEntryId] = field;
            }

        }

        /// <inheritdoc />
        public void UpdateField(string holderType, string holderId, string fieldName, string serializedFieldValue, string language = "en")
        {
            var field = this.GetField(holderType, holderId, fieldName, language);

            field.FieldValue = serializedFieldValue;
            field.LastUpdatedUtc = DateTime.UtcNow;

            this.UpdateFieldInDb(field);

            if (Configuration.CacheEnabled)
            {
                var cacheEntryId = CacheEntryIdFormatter(holderType, holderId, fieldName, language);

                this.FieldCache.Value[cacheEntryId] = field;
            }
        }

        #endregion

        #region Abstract Methods

        /// <inheritdoc />
        public abstract Field GetFieldFromDb(string holderType, string holderId, string fieldName, string language = "en");

        /// <inheritdoc />
        public abstract void AddFieldValueToDb(Field field);

        /// <inheritdoc />
        public abstract void UpdateFieldInDb(Field field);

        #endregion

        #region Private Methods

        private string CacheEntryIdFormatter(string holderType, string holderId, string fieldName, string language = "en")
        {
            return $"{holderType}_{holderId}_{fieldName}_{language}";
        }

        #endregion
    }
}
