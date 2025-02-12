using System.ComponentModel.DataAnnotations;

namespace ChloeOS.Client.Models;

public class SignInCredentials {

    [Required]
    public string Username { get; set; }

    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}