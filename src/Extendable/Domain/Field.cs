using System;

namespace Extendable.Domain
{
    public sealed class Field
    {
        public string Id { get; set; }

        public string HolderType { get; set; }

        public string HolderId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { set; get; }

        public string Language { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }
}