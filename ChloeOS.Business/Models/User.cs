using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChloeOS.Business.Models;

[Table("os_users")]
public class User {

    [Key]
    [Column("id")]
    public Ulid Id { get; set; } = Ulid.NewUlid();

    [Required(ErrorMessage = "A username is required.")]
    [Column("username")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Your first name is required.")]
    [Column("first_name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Your last name is required.")]
    [Column("last_name")]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Required]
    [Column("password_salt")]
    public string PasswordSalt { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}