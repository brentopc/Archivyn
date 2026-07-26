namespace Archivyn.Models;

public class KeywordSetKeyType
{
    public long KeySetTableNum { get; set; }

    public long KeyTypeNum { get; set; }

    public int DisplayOrder { get; set; }

    public KeywordSet KeywordSet { get; set; } = null!;

    public KeyType KeyType { get; set; } = null!;
}