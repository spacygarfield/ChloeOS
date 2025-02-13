using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Core.Models.FS;

public class File {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(FileMetadata))]
    public Guid FileMetadataId { get; set; }

    public byte[] Bytes { get; set; } = [];


}