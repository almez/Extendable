using System;
using System.Collections.Generic;
using System.Linq;
using Extendable.Domain;
using Extendable.Providers;

namespace Extendable.Tests.Common
{
    public class InMemoryFieldProvider : BaseFieldProvider
    {
        #region Fields

        private readonly List<Field> _dbTable = new List<Field>();

        #endregion

        #region Public Methods

        public override void UpdateFieldInDb(Field field)
        {
            if (_dbTable.Any(x => x.Id == field.Id))
            {
                _dbTable.Remove(_dbTable.Single(x => x.Id == field.Id));
                _dbTable.Add(field);
            }
        }

        public override IQueryable<Field> QueryAllFields()
        {
            return _dbTable.AsQueryable();
        }

        public override Field GetFieldFromDb(string holderType, string holderId, string fieldName, string language = "en")
        {
            return _dbTable.FirstOrDefault(x => x.HolderType == holderType &&
                                                x.HolderId == holderId &&
                                                x.FieldName == fieldName &&
                                                x.Language == language);
        }

        public override void AddFieldValueToDb(Field field)
        {
            _dbTable.Add(field);
        }

        #endregion
    }
}
