using System.Linq;
using Extendable.Domain;

namespace Extendable.Abstraction
{
    public interface IFieldProvider
    {
        #region Public Methods

        /// <summary>
        /// Add or Update dynamic field process which includes updating cache entry, raise event ..etc
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="fieldValue">the dynaic field's value</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        void AddOrUpdateField<TValue>(string holderType, string holderId, string fieldName, TValue fieldValue, string language = "en");

        /// <summary>
        /// Add new dynamic field process which includes updating cache entry, raise event ..etc
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="serializedFieldValue">the serialized dynamic field value as string</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        void AddField(string holderType, string holderId, string fieldName, string serializedFieldValue, string language = "en");

        /// <summary>
        /// Update field process, which includes cache update, database update ..etc
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="serializedFieldValue">the serialized dynamic field value as string</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        void UpdateField(string holderType, string holderId, string fieldName, string serializedFieldValue, string language = "en");

        /// <summary>
        /// Get Dynamic field, it will tries to get the value from cache first.
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        /// <returns></returns>
        Field GetField(string holderType, string holderId, string fieldName, string language = "en");

        /// <summary>
        /// Check whether the dynamic field that matches the specified parameter is existed or not
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        /// <returns></returns>
        bool IsExist(string holderType, string holderId, string fieldName, string language = "en");

        /// <summary>
        /// Get Dynamic field value, it will tries to get the value from cache first.
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="defaultValue">The value returned if no existing value there</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        /// <returns></returns>
        TValue GetFieldValue<TValue>(string holderType, string holderId, string fieldName, TValue defaultValue = default(TValue), string language = "en");

        /// <summary>
        /// Clear any cached fields
        /// </summary>
        void ClearCachedFields();

        /// <summary>
        /// Get Saved Fields as IQueryable
        /// </summary>
        IQueryable<Field> QueryAllFields();
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Add new field record to the database or any permanent store
        /// </summary>
        /// <param name="field">field object</param>
        void AddFieldValueToDb(Field field);

        /// <summary>
        /// Update field record in the database or any permanent store
        /// </summary>
        /// <param name="field">field object</param>
        void UpdateFieldInDb(Field field);

        /// <summary>
        /// Update field frmo the database or any permanent store
        /// </summary>
        /// <param name="holderType">The type name that contains this dynamic field</param>
        /// <param name="holderId">The type isntance that contains this dynamic field</param>
        /// <param name="fieldName">The dynamic field name</param>
        /// <param name="language">in-case of your system in multi-language, this parameter represents language code</param>
        Field GetFieldFromDb(string holderType, string holderId, string fieldName, string language = "en");

        #endregion
    }
}
