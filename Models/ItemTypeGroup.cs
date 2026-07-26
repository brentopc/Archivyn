using System.Xml.Linq;

namespace Archivyn.Models;

public sealed class ItemTypeGroup
{
    public long ItemTypeGroupNum { get; set; }

    public string ItemTypeGroupName { get; set; } = string.Empty;

    //public long InUse { get; set; } = 1;

    //public long ItemTypeGroupUsed { get; set; }

    //public long NumRows { get; set; }

   // public long DocSourceFlag { get; set; }

    //public long DiskGroupNum { get; set; }

    public long Flags { get; set; }

    public ICollection<DocumentType> DocumentTypes { get; set; }
        = new List<DocumentType>();
}