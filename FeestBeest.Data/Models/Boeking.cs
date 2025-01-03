using System;
using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Models;

public class Boeking
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }

    [Required]
    [MaxLength(100)]
    public string ContactNaam { get; set; }

    [MaxLength(200)]
    public string? ContactAdres { get; set; }

    [MaxLength(100)]
    public string? ContactEmail { get; set; }

    [MaxLength(15)]
    public string? ContactTelefoonnummer { get; set; }

    [Required]
    [Range(0, 9999.99)]
    public decimal TotaalPrijs { get; set; }

    public int? AccountId { get; set; }
    public Account? Account { get; set; }

    public int? BeestjeId { get; set; } // Add this property to reference Beestje
    public Beestje? Beestje { get; set; } // Add this property to reference Beestje
}