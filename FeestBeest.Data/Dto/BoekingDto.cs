namespace FeestBeest.Data.Dto;

public class BoekingDto
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string ContactNaam { get; set; }
    public string ContactAdres { get; set; }
    public string ContactEmail { get; set; }
    public string ContactTelefoonnummer { get; set; }
    public decimal TotaalPrijs { get; set; }
    public bool IsBevestigd { get; set; } // Add this property
    public List<BeestjeDto> Beestjes { get; set; }
}