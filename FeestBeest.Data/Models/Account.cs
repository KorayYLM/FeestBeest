using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeestBeest.Data.Models
{
    public class Account : IdentityUser<int>
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam mag niet langer zijn dan 100 tekens.")]
        public string Naam { get; set; }

        [StringLength(200, ErrorMessage = "Adres mag niet langer zijn dan 200 tekens.")]
        public string? Adres { get; set; }

        [Required(ErrorMessage = "Email is verplicht.")]
        [EmailAddress(ErrorMessage = "Voer een geldig emailadres in.")]
        public override string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefoonnummer moet precies 10 cijfers bevatten.")]
        [StringLength(10, ErrorMessage = "Telefoonnummer mag niet langer zijn dan 10 cijfers.")]
        public string? Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Klanttype is verplicht.")]
        public KlantenkaartType KlantType { get; set; }

        public ICollection<Boeking> Boekingen { get; set; } = new List<Boeking>();
    }

    public enum KlantenkaartType
    {
        
        [Display(Name = "Zilver")]
        Geen,
        
        [Display(Name = "Zilver")]
        Zilver,

        [Display(Name = "Goud")]
        Goud,

        [Display(Name = "Platina")]
        Platina
    }
}