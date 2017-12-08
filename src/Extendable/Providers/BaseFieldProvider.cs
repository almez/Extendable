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

        private Lazy<Cache<Field, string>> _fieldCache = new Lazy<Cache<Field, string>>(() =>
        {
            var cacheName = "Fields Cache";
            var cache =  CacheManager.Instance.FindCacheByName(cacheName);
            if (cache == null)
            {
                cache = CacheFactory.CreateCache<Field, string>(cacheName, Configuration.CacheSizeLimit);
            }
            return (Cache<Field, string>) cache;
        });

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void AddOrUpdateField<TValue>(string holderType, string holderId, string fieldName, TValue fieldValue, string language = "en")
        {
            var serializedFieldValue = fieldValue.ToString();

            if (this.Exists(holderType, holderId, fieldName, language))
            {
                this.UpdateField(holderType, holderId, fieldName, serializedFieldValue, language);
            }
            else
            {
                this.AddField(holderType, holderId, fieldName, serializedFieldValue, language);
            }
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
        public bool Exists(string holderType, string holderId, string fieldName, string language = "en")
        {
            return this.GetField(holderType, holderId, fieldName, language) != null;
        }

        /// <inheritdoc />
        public void ClearCachedFields()
        {
            if (Configuration.CacheEnabled)
            {
                this._fieldCache.Value.Clear();
            }
        }

        /// <inheritdoc />
        public Field GetField(string holderType, string holderId, string fieldName, string language = "en")
        {
            Field field = null;

            if (Configuration.CacheEnabled)
            {
                var cacheEntryId = CacheEntryIdFormatter(holderType, holderId, fieldName, language);

                field = this._fieldCache.Value[cacheEntryId];
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

                this._fieldCache.Value[cacheEntryId] = field;
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

                this._fieldCache.Value[cacheEntryId] = field;
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

        ///// <inheritdoc />
        //public abstract IQueryable<Field> QueryAllFields();
        #endregion

        #region Private Methods

        private string CacheEntryIdFormatter(string holderType, string holderId, string fieldName, string language = "en")
        {
            return $"{holderType}_{holderId}_{fieldName}_{language}";
        }

        #endregion
    }
}
