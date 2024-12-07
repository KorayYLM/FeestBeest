using System;
using System.Collections.Generic;

public class Boeking
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string ContactNaam { get; set; }
    public string ContactAdres { get; set; }
    public string ContactEmail { get; set; }
    public string ContactTelefoonnummer { get; set; }
    public bool IsBevestigd { get; set; }
    public decimal TotaalPrijs { get; set; }
    public ICollection<Beestje> Beestjes { get; set; } = new List<Beestje>();
    public int? AccountId { get; set; }
    public Account? Account { get; set; }
}