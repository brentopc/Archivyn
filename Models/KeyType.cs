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
    }
}
