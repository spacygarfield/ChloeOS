using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChloeOS.Core.Models.OS;

namespace ChloeOS.Core.Models.FS;

public class FileMetadata {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(User))]
    public Guid OwnerId { get; set; }

    [Required(ErrorMessage = "A filename is required.")]
    public string Filename { get; set; }

    [NotMapped]
    public string FilePath { get; set; } // Will be generated via the attached file's relationships.

    [Required]
    public string FileExtension { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? ModifiedAt { get; set; }

    public DateTime? AccessedAt { get; set; }

    [Required]
    public bool IsReadOnly { get; set; } = false;

    [Required]
    public bool IsHidden { get; set; } = false;

    public virtual User Owner { get; set; } = null!;

}