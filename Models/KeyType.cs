namespace Archivyn.Models
{
    public class KeyType
    {
        public long KeyTypeNum { get; set; }

        public string KeyTypeName { get; set; } = string.Empty;

        public string? KeyTypeMask { get; set; }

        public long KeyTypeFlags { get; set; }

        public long DataType { get; set; }

        public long KeyTypeLen { get; set; }

        public ICollection<KeywordSetKeyType> KeywordSetMemberships { get; set; }
        = new List<KeywordSetKeyType>();

        public ICollection<DocumentTypeKeyType> DocumentTypeAssignments { get; set; }
        = new List<DocumentTypeKeyType>();
    }
}
