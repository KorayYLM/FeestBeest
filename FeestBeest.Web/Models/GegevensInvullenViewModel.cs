public class GegevensInvullenViewModel
{
    public DateTime SelectedDate { get; set; } 
    
    public List<Beestje> SelectedBeestjes { get; set; } = new List<Beestje>();
    
    public string ContactNaam { get; set; }
    
    public string ContactAdres { get; set; }
    
    public string ContactEmail { get; set; }
    
    public string ContactTelefoonnummer { get; set; }
    
}