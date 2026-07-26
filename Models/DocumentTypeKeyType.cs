namespace Archivyn.Models;

public sealed class DocumentTypeKeyType
{
    public long ItemTypeNum { get; set; }

    public long KeyTypeNum { get; set; }

    public int DisplayOrder { get; set; }

    public DocumentType DocumentType { get; set; } = null!;

    public KeyType KeyType { get; set; } = null!;
}