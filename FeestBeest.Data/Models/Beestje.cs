using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Beestje
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Naam { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; }

    [Required]
    [Range(0, 9999.99)]
    public decimal Prijs { get; set; }

    [Required]
    [Url]
    public string Afbeelding { get; set; } 

    public ICollection<Boeking> Boekingen { get; set; } = new List<Boeking>();
}