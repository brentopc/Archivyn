namespace Archivyn.Models;

public sealed class KeyItem
{
    public long KeyItemNum { get; set; }

    public long ItemNum { get; set; }

    public long KeyTypeNum { get; set; }

    // Used later for keyword groups and multi-instance groups.
    public long? KeySetTableNum { get; set; }

    public long? RecordNum { get; set; }

    public string? KeyValueChar { get; set; }

    public decimal? KeyValueNum { get; set; }

    public DateTime? KeyValueDate { get; set; }

    public ItemData ItemData { get; set; } = null!;

    public KeyType KeyType { get; set; } = null!;
}