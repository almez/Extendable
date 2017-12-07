using System;

namespace Extendable.Domain
{
    internal sealed class Field
    {
        public int Id { get; set; }

        public string HolderType { get; set; }

        public string HolderId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { set; get; }

        public string Language { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }
}