using FeestBeest.Data.Dto;
using FeestBeest.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

public class BoekingViewModel
{
    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public string ContactNaam { get; set; }
    public string ContactAdres { get; set; }
    public string ContactEmail { get; set; }
    public string ContactTelefoonnummer { get; set; }
    public decimal TotaalPrijs { get; set; }
    public decimal Korting { get; set; } // Add this property
    public decimal TeBetalen { get; set; } // Add this property
    public bool IsBevestigd { get; set; }
    public List<BeestjeViewModel> Beestjes { get; set; }

    public static BoekingViewModel FromDto(BoekingDto dto)
    {
        return new BoekingViewModel
        {
            Id = dto.Id,
            Datum = dto.Datum,
            ContactNaam = dto.ContactNaam,
            ContactAdres = dto.ContactAdres,
            ContactEmail = dto.ContactEmail,
            ContactTelefoonnummer = dto.ContactTelefoonnummer,
            TotaalPrijs = dto.TotaalPrijs,
            Korting = dto.Korting, // Add this line
            TeBetalen = dto.TeBetalen, // Add this line
            IsBevestigd = dto.IsBevestigd,
            Beestjes = dto.Beestjes.Select(BeestjeViewModel.FromDto).ToList()
        };
    }

    public BoekingDto ToDto()
    {
        return new BoekingDto
        {
            Id = this.Id,
            Datum = this.Datum,
            ContactNaam = this.ContactNaam,
            ContactAdres = this.ContactAdres,
            ContactEmail = this.ContactEmail,
            ContactTelefoonnummer = this.ContactTelefoonnummer,
            TotaalPrijs = this.TotaalPrijs,
            Korting = this.Korting, // Add this line
            TeBetalen = this.TeBetalen, // Add this line
            IsBevestigd = this.IsBevestigd,
            Beestjes = this.Beestjes.Select(b => b.ToDto()).ToList()
        };
    }
}