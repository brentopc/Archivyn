namespace Archivyn.Models;

public sealed class DocumentTypeKeywordTypeGroup : ISystemManagedEntity
{
    public bool IsSystem { get; private set; }
    
    public long ItemTypeNum { get; set; }

    public long KeySetTableNum { get; set; }

    public int DisplayOrder { get; set; }

    public DocumentType DocumentType { get; set; } = null!;

    public KeywordSet KeywordTypeGroup { get; set; } = null!;
}