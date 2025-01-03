using System.ComponentModel.DataAnnotations;

namespace FeestBeest.Web.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string CustomerCard { get; set; }

    [Required]
    public string Adres { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
}
