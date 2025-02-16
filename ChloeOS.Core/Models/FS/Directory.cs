using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChloeOS.Core.Models.OS;

namespace ChloeOS.Core.Models.FS;

public class Directory {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Directory))]
    public Guid? ParentId { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public Guid OwnerId { get; set; }

    [Required(ErrorMessage = "A folder name is required.")]
    public string Name { get; set; }

    [NotMapped]
    public string FilePath { get; set; } // Will be generated via the attached file's relationships.

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? ModifiedAt { get; set; }

    public DateTime? AccessedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsReadOnly { get; set; }

    [DefaultValue(false)]
    public bool IsHidden { get; set; }

    public virtual Directory Parent { get; set; } = null!;
    public virtual User Owner { get; set; } = null!;
    public virtual ICollection<Directory> SubDirectories { get; set; } = new HashSet<Directory>();
    public virtual ICollection<File> SubFiles { get; set; } = new HashSet<File>();

}