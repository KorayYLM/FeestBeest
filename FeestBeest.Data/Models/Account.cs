using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class Account : IdentityUser
{
    public string Naam { get; set; }
    public string Adres { get; set; }
    public string Telefoonnummer { get; set; }
    public KlantenkaartType? Klantenkaart { get; set; }
    public ICollection<Boeking> Boekingen { get; set; } = new List<Boeking>();
}

public enum KlantenkaartType
{
    Zilver,
    Goud,
    Platina
}