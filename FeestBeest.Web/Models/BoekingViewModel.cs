using System;
using System.ComponentModel.DataAnnotations;

public class BoekingViewModel
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }

    [Required(ErrorMessage = "Contactnaam is verplicht.")]
    [MaxLength(100, ErrorMessage = "Contactnaam mag niet langer zijn dan 100 tekens.")]
    public string ContactNaam { get; set; }

    [MaxLength(200, ErrorMessage = "Contactadres mag niet langer zijn dan 200 tekens.")]
    public string? ContactAdres { get; set; }

    [MaxLength(100, ErrorMessage = "Contactemail mag niet langer zijn dan 100 tekens.")]
    public string? ContactEmail { get; set; }

    [MaxLength(15, ErrorMessage = "Contacttelefoonnummer mag niet langer zijn dan 15 tekens.")]
    public string? ContactTelefoonnummer { get; set; }

    [Required(ErrorMessage = "Totaalprijs is verplicht.")]
    [Range(0, 9999.99, ErrorMessage = "Totaalprijs moet tussen 0 en 9999.99 liggen.")]
    public decimal TotaalPrijs { get; set; }

    public string? AccountId { get; set; }
    public int? BeestjeId { get; set; }
}