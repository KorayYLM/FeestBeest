using System.Collections.Generic;

public class Beestje
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Type { get; set; }
    public decimal Prijs { get; set; }
    public string Afbeelding { get; set; } // URL naar de afbeelding
    public ICollection<Boeking> Boekingen { get; set; } = new List<Boeking>();
}