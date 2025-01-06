using FeestBeest.Data.Dto;

namespace FeestBeest.Web.Models;

public class BoekingIndexViewModel
{
    public DateTime SelectedDate { get; set; }
    public List<Beestje> SelectedBeestjes { get; set; } = new List<Beestje>();
    public List<BoekingViewModel> Boekingen { get; set; }
    public List<Beestje> Beestjes { get; set; }
}