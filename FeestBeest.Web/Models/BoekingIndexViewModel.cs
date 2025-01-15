﻿using FeestBeest.Data.Dto;
using System;
using System.Collections.Generic;

namespace FeestBeest.Web.Models
{
    public class BoekingIndexViewModel
    {
        public DateTime SelectedDate { get; set; }
        public List<int> SelectedBeestjesIds { get; set; } = new List<int>();
        public List<Product> SelectedBeestjes { get; set; } = new List<Product>();
        public List<OrderViewModel> Boekingen { get; set; }
        public List<Product> Beestjes { get; set; }
        
        public int Id { get; set; }
        public string ContactNaam { get; set; }
        public string ContactAdres { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelefoonnummer { get; set; }
        public decimal TotaalPrijs { get; set; }
    }
}