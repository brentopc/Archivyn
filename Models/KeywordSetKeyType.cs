namespace Archivyn.Models;

public class KeywordSetKeyType : ISystemManagedEntity
{
    public bool IsSystem { get; private set; }
    
    public long KeySetTableNum { get; set; }

    public long KeyTypeNum { get; set; }

    public int DisplayOrder { get; set; }

    public KeywordSet KeywordSet { get; set; } = null!;

    public KeyType KeyType { get; set; } = null!;
}