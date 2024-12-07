public class Account
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Adres { get; set; }
    public string Email { get; set; }
    public string Telefoonnummer { get; set; }
    public string Wachtwoord { get; set; }
    public KlantenkaartType? Klantenkaart { get; set; }
}

public enum KlantenkaartType
{
    Zilver,
    Goud,
    Platina
}