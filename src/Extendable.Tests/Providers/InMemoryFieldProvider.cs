using System.Collections.Generic;
using System.Linq;
using Extendable.Domain;
using Extendable.Providers;

namespace Extendable.Tests.Providers
{
    public class InMemoryFieldProvider : BaseFieldProvider
    {
        #region Fields

        public readonly List<Field> DbTable = new List<Field>();

        #endregion

        #region Public Methods

        public override void UpdateFieldInDb(Field field)
        {
            if (this.DbTable.Any(x => x.Id == field.Id))
            {
                this.DbTable.Remove(this.DbTable.Single(x => x.Id == field.Id));
                this.DbTable.Add(field);
            }
        }

        public List<Field> ListFields()
        {
            return this.DbTable;
        }

        public override Field GetFieldFromDb(string holderType, string holderId, string fieldName, string language = "en")
        {
            return this.DbTable.FirstOrDefault(x => x.HolderType == holderType &&
                                                x.HolderId == holderId &&
                                                x.FieldName == fieldName &&
                                                x.Language == language);
        }

        public override void AddFieldValueToDb(Field field)
        {
            this.DbTable.Add(field);
        }

        #endregion
    }
}
