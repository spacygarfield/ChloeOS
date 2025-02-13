using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChloeOS.Core.Models.Misc;

namespace ChloeOS.Core.Models.OS;

public class User {

    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "A username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Your first name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Your last name is required.")]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string PasswordSalt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public void SetPassword(Password password) {
        PasswordHash = Convert.ToBase64String(password.Hash);
        PasswordSalt = Convert.ToBase64String(password.Salt);
    }

}