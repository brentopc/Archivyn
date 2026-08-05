namespace Archivyn.Models;

public sealed class ItemData
{
    public long ItemNum { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public int Status { get; set; }

    public long ItemTypeGroupNum { get; set; }

    public long ItemTypeNum { get; set; }

    public DateTime ItemDate { get; set; }

    public DateTime DateStored { get; set; }

    public long? UserNum { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    //public string ContentType { get; set; } = "application/octet-stream";

    public long FileSize { get; set; }

    public DocumentType DocumentType { get; set; } = null!;

    public ItemTypeGroup ItemTypeGroup { get; set; } = null!;

    public ICollection<KeyItem> KeywordValues { get; set; }
        = new List<KeyItem>();
}