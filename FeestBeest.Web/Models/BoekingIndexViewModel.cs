using FeestBeest.Data.Dto;
using System;
using System.Collections.Generic;

namespace FeestBeest.Web.Models
{
    public class BoekingIndexViewModel
    {
        public DateTime SelectedDate { get; set; }
        public List<int> SelectedBeestjesIds { get; set; } = new List<int>();
        public List<Beestje> SelectedBeestjes { get; set; } = new List<Beestje>();
        public List<BoekingViewModel> Boekingen { get; set; }
        public List<Beestje> Beestjes { get; set; }
    }
}