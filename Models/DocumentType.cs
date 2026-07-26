namespace Archivyn.Models;

public sealed class DocumentType
{
    public long ItemTypeNum { get; set; }

    public string ItemTypeName { get; set; } = string.Empty;

    //public long ItRevNum { get; set; }

    public long ItemTypeGroupNum { get; set; }

    public ItemTypeGroup ItemTypeGroup { get; set; } = null!;

    //public long FileTypeNum { get; set; }

    //public long CompressFile { get; set; }

    public string? AutoNameString { get; set; }

    //public long InUse { get; set; } = 1;

    //public long DiskGroupNum { get; set; }

    //public long DisplayThumbs { get; set; }

    //public long NumRows { get; set; }

    //public long IsDocRevisionable { get; set; }

    //public long DocSourceFlag { get; set; }

    //public long ImageWindowFlags { get; set; }

    //public long UiFlags { get; set; }

    //public long ItemTypeFlags { get; set; }

    //public long RevisableByInst { get; set; }

    //public long ItemTypeFlags2 { get; set; }

    public ICollection<DocumentTypeKeyType> KeywordTypeAssignments { get; set; }
    = new List<DocumentTypeKeyType>();

    public ICollection<DocumentTypeKeywordTypeGroup> KeywordTypeGroupAssignments { get; set; }
        = new List<DocumentTypeKeywordTypeGroup>();
}