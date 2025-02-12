using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Domain.Models;

[Table("fs_filemetadata")]
public class FileMetadata {

    [Key]
    [Column("id")]
    public Ulid Id { get; set; } = Ulid.NewUlid();

    [Required(ErrorMessage = "A filename is required.")]
    [Column("filename")]
    public string Filename { get; set; }

    [Required]
    [Column("path")]
    public string FilePath { get; set; }

    [Required]
    [Column("extension")]
    public string FileExtension { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("accessed_at")]
    public DateTime? AccessedAt { get; set; }

    [Required]
    [Column("read_only")]
    public bool IsReadOnly { get; set; } = false;

    [Required]
    [Column("hidden")]
    public bool IsHidden { get; set; } = false;

    [ForeignKey("owner_id")]
    public Ulid OwnerId { get; set; }

    public virtual User Owner { get; set; } = null!;

}