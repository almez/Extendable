using System;
using System.Collections.Generic;
using System.Linq;
using Extendable.Abstraction;
using Extendable.Domain;
using Extendable.Tests.Domain;
using Extendable.Tests.Providers;
using Xunit;

namespace Extendable.Tests
{
    public class BaseFieldProviderTest
    {
        #region Fields

        private InMemoryFieldProvider _fieldProvider;

        #endregion

        #region Setup

        void Initialize()
        {
            this._fieldProvider = new InMemoryFieldProvider();
            this._fieldProvider.ClearCachedFields();
        }

        #endregion

        #region FieldProvider.AddOrUpdate()

        [Fact(DisplayName = "FieldProvider: AddOrUpdate adds new field")]
        public void AddOrUpdateField_WithNewField_AddsField()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //var field = FieldFactory.CreateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            var totalFields = _fieldProvider.DbTable.Count;

            //Assert
            Assert.Equal(totalFields, 1);
        }

        [Fact(DisplayName = "FieldProvider: AddOrUpdate updates existing field")]
        public void AddOrUpdateField_WithExistingField_UpdatesField()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz1");
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz2");
            var totalFields = _fieldProvider.DbTable.Count;

            //Assert
            Assert.Equal(totalFields, 1);
        }

        #endregion

        #region FieldProvider.Exists()

        [Fact(DisplayName = "FieldProvider: Exists returns true with existed field")]
        public void ExistsMethod_WithExistedField_ReturnsTrue()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz", "ar");

            //Assert
            var enResult = _fieldProvider.Exists(typeof(User).Name, user.Id.ToString(), "LastName", "en");
            var arResult = _fieldProvider.Exists(typeof(User).Name, user.Id.ToString(), "LastName", "ar");

            Assert.True(enResult);
            Assert.True(arResult);
        }

        [Fact(DisplayName = "FieldProvider: Exists returns false with non-existed field")]
        public void ExistsMethod_WithNonExistedField_ReturnsFalse()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            var isExisted = _fieldProvider.Exists(typeof(User).Name, user.Id.ToString(), "LastName");

            //Assert
            Assert.False(isExisted);
        }

        #endregion

        #region FieldProvider.GetFieldValue()

        [Fact(DisplayName = "FieldProvider: GetFieldValue returns proper value if existed")]
        public void GetFieldValue_WithExistingField_ReturnsProperValue()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "منز", "ar");

            //Assert
            var enResult = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName", null, "en");
            var arResult = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName", null, "ar");

            Assert.Equal(enResult, "Menz");
            Assert.Equal(arResult, "منز");
        }

        [Fact(DisplayName = "FieldProvider: GetFieldValue returns default value when field not existed")]
        public void GetFieldValue_WithNonExistedField_ReturnsDefaultValue()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act


            //Assert
            var enResult = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName", "EN DEFAULT", "en");
            var arResult = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName", "AR DEFAULT", "ar");

            Assert.Equal(enResult, "EN DEFAULT");
            Assert.Equal(arResult, "AR DEFAULT");
        }

        #endregion

        #region FieldProvider.ClearCachedFields

        [Fact(DisplayName = "FieldProvider: ClearCachedFields clears entries from internal cache")]
        public void ClearCachedFields_CacheEnabled_GetNExtRequestFromDb()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            _fieldProvider.ClearCachedFields();
            _fieldProvider.DbTable.Clear();

            //Assert
            var result = _fieldProvider.GetField(typeof(User).Name, user.Id.ToString(), "LastName");

            Assert.Null(result);
        }

        #endregion

        #region FieldProvider.GetField()

        [Fact(DisplayName = "FieldProvider: GetField tries to get value from cache first")]
        public void GetField_CacheEnabled_TriesToGetValueFromCacheFirst()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddOrUpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            _fieldProvider.DbTable.Clear();

            //Assert
            var result = _fieldProvider.GetField(typeof(User).Name, user.Id.ToString(), "LastName");

            Assert.NotNull(result);
            Assert.Equal(result?.FieldValue, "Menz");
        }

        #endregion

        #region FieldProvider.AddField()

        [Fact(DisplayName = "FieldProvider: AddField() adds field to db")]
        public void AddField_ValidInput_AddsFieldToDb()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");

            //Assert
            var totalCount = _fieldProvider.DbTable.Count;

            Assert.Equal(totalCount, 1);
        }

        [Fact(DisplayName = "FieldProvider: AddField() adds field to cache")]
        public void AddField_CacheEnabled_AddsFieldToCache()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");
            _fieldProvider.DbTable.Clear();

            //Assert
            var result = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName");

            Assert.Equal(result, "Menz");
        }

        #endregion

        #region FieldProvider.UpdateField()

        [Fact(DisplayName = "FieldProvider: UpdateField() updates field in db")]
        public void UpdateField_WithExistingField_UpdatesFieldInDb()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");

            _fieldProvider.UpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz2");

            //Assert
            var result = _fieldProvider.DbTable.FirstOrDefault(x => x.HolderType == typeof(User).Name
                                                                    && x.HolderId == user.Id.ToString() &&
                                                                    x.FieldName == "LastName")?.FieldValue;

            Assert.Equal(result, "Menz2");
        }

        [Fact(DisplayName = "FieldProvider: UpdateField() updates field in cache")]
        public void UpdateField_WithExistingField_UpdatesFieldInCache()
        {
            //Arrange
            this.Initialize();

            var user = new User() { Id = 10, Name = "Alden" };

            //Act
            _fieldProvider.AddField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz");

            _fieldProvider.UpdateField(typeof(User).Name, user.Id.ToString(), "LastName", "Menz2");

            _fieldProvider.DbTable.Clear();

            //Assert
            var result = _fieldProvider.GetFieldValue<string>(typeof(User).Name, user.Id.ToString(), "LastName");

            Assert.Equal(result, "Menz2");
        }

        #endregion
    }
}
