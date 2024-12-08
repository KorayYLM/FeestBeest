using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeestBeest.Data.Models;

public class Account : IdentityUser
{
    public string Id { get; set; } // Ensure type is string

    [Required]
    [MaxLength(100)]
    public string Naam { get; set; }

    [MaxLength(200)]
    public string? Adres { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(15)]
    public string? Telefoonnummer { get; set; }

    public ICollection<Boeking> Boekingen { get; set; } = new List<Boeking>();
}

public enum KlantenkaartType
{
    Zilver,
    Goud,
    Platina
}