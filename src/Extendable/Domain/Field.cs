using System;
using CachingManager.Abstraction;
using Extendable.Utils;


namespace Extendable.Domain
{
    public sealed class Field : IMesurable
    {
        #region Public Properties

        public string Id { get; set; }

        public string HolderType { get; set; }

        public string HolderId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { set; get; }

        public string Language { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }

        #endregion


        #region IMesurable Implementation

        public long GetSizeInBytes()
        {
            return TypeUtil.IntSize +
                   TypeUtil.StringSize(this.HolderType) +
                   TypeUtil.StringSize(this.HolderId) +
                   TypeUtil.StringSize(this.FieldName) +
                   TypeUtil.StringSize(this.FieldValue) +
                   TypeUtil.StringSize(this.Language);
        }

        #endregion
    }
}