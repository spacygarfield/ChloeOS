using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Core.Models.FS;

public class File {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(FileMetadata))]
    public Guid MetadataId { get; set; }

    [ForeignKey(nameof(Folder))]
    public Guid FolderId { get; set; }

    public byte[] Bytes { get; set; } = [];

    public virtual FileMetadata Metadata { get; set; } = null!;
    public virtual Folder Parent { get; set; } = null!;

}