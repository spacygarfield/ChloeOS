using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Core.Models.FS;

public class Folder {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(FolderMetadata))]
    public Guid MetadataId { get; set; }

    [ForeignKey(nameof(Folder))]
    public Guid ParentFolderId { get; set; }

    public virtual FolderMetadata Metadata { get; set; } = null!;
    public virtual Folder Parent { get; set; } = null!;
    public virtual ICollection<Folder> SubFolders { get; set; } = new HashSet<Folder>();
    public virtual ICollection<File> SubFiles { get; set; } = new HashSet<File>();

}