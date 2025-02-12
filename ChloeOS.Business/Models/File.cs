using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Business.Models;

[Table("fs_files")]
public class File {

    [Key]
    [Column("id")]
    public Ulid Id { get; set; } = Ulid.NewUlid();

    [ForeignKey(nameof(FileMetadata))]
    [Column("metadata_id")]
    public Ulid FileMetadataId { get; set; }

    [Column("bytes")]
    public byte[] Bytes { get; set; } = [];


}