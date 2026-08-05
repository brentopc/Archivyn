namespace Archivyn.Models
{
    public class KeyType : ISystemManagedEntity
    {
        public bool IsSystem { get; private set; }
        public bool AddToAllDocumentTypes { get; private set; }
        public bool IsRequiredOnAllDocumentTypes { get; private set; }
        public int AllDocumentTypesDisplayOrder { get; private set; } = 0;


        public long KeyTypeNum { get; set; }

        public string KeyTypeName { get; set; } = string.Empty;

        //public string? KeyTypeMask { get; set; }

        //public long KeyTypeFlags { get; set; }

        public int DataType { get; set; } = 1;

        public long KeyTypeLen { get; set; }

        public ICollection<KeywordSetKeyType> KeywordSetMemberships { get; set; }
        = new List<KeywordSetKeyType>();

        public ICollection<DocumentTypeKeyType> DocumentTypeAssignments { get; set; }
        = new List<DocumentTypeKeyType>();

        public static class DataTypes
        {
            public const int Text = 1;
            public const int Number = 2;
            public const int Date = 3;
        }
    }
}
