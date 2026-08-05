namespace Archivyn.Models;

public sealed class DocumentTypeKeyType : ISystemManagedEntity
{
    public bool IsSystem { get; private set; }

    public long ItemTypeNum { get; set; }

    public long KeyTypeNum { get; set; }

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }

    public DocumentType DocumentType { get; set; } = null!;

    public KeyType KeyType { get; set; } = null!;

    public static DocumentTypeKeyType CreateSystemAssignment(DocumentType documentType, KeyType keywordType)
    {
        return new DocumentTypeKeyType
        {
            DocumentType = documentType,
            ItemTypeNum = documentType.ItemTypeNum,
            KeyTypeNum = keywordType.KeyTypeNum,
            DisplayOrder = keywordType.AllDocumentTypesDisplayOrder,
            IsRequired = keywordType.IsRequiredOnAllDocumentTypes,
            IsSystem = true
        };
    }
}