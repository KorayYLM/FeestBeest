using FeestBeest.Data.Models;

namespace FeestBeest.Data.Dto;

public class ContactInfo
{
    public int Id { get; set; }
    public DateTime SelectedDate { get; set; }
    public List<Product> SelectedBeestjes { get; set; }
    public string ContactNaam { get; set; }
    public string ContactAdres { get; set; }
    public string ContactEmail { get; set; }
    public string ContactTelefoonnummer { get; set; }
}