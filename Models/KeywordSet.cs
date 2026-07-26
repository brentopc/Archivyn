namespace Archivyn.Models;

public sealed class KeywordSet
{
    public long KeySetTableNum { get; set; }

    public string KeySetName { get; set; } = string.Empty;

    public long IsKeyTypeGroup { get; set; }

    public long Flags { get; set; }

    public ICollection<KeywordSetKeyType> KeywordTypeMemberships { get; set; }
         = new List<KeywordSetKeyType>();

    public ICollection<DocumentTypeKeywordTypeGroup> DocumentTypeAssignments
    { get; set; } = new List<DocumentTypeKeywordTypeGroup>();
}