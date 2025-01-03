using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Models;

public class AccountViewModel
{
    
    public string Id { get; set; }  
    
    [Required(ErrorMessage = "Naam is verplicht.")]
    public string Naam { get; set; }

    [Required(ErrorMessage = "Adres is verplicht.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Adres mag alleen letters en spaties bevatten.")]
    public string Adres { get; set; }

    [Required(ErrorMessage = "Email is verplicht.")]
    [EmailAddress(ErrorMessage = "Ongeldig emailadres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefoonnummer moet precies 10 cijfers bevatten.")]
    public string Telefoonnummer { get; set; }

    [Required(ErrorMessage = "Klanttype is verplicht.")]
    public string KlantType { get; set; } // Houd als string voor formulier binding
}