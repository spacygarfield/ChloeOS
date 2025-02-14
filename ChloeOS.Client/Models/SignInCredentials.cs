using System.ComponentModel.DataAnnotations;

namespace ChloeOS.Client.Models;

public class SignInCredentials {

    [Required(ErrorMessage = "No local user was selected. Please try again.")]
    [Display(Name = "Please select a user")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Please enter your password.")]
    [Display(Name = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}