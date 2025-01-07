// File: FeestBeest.Data/Services/BoekingService.cs
using FeestBeest.Data;
using FeestBeest.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeestBeest.Data.Services;

public class BoekingService : IBoekingService
{
    private readonly FeestBeestContext _context;

    public BoekingService(FeestBeestContext context)
    {
        _context = context;
    }

    public async Task<List<BeestjeDto>> GetBeschikbareBeestjesAsync(DateTime datum)
    {
        var geboekteBeestjes = await _context.Boekingen
            .Where(b => b.Datum == datum)
            .SelectMany(b => b.Beestjes)
            .ToListAsync();

        var beschikbareBeestjes = await _context.Beestjes
            .Where(b => !geboekteBeestjes.Contains(b))
            .ToListAsync();

        return beschikbareBeestjes.Select(b => new BeestjeDto
        {
            Id = b.Id,
            Naam = b.Naam,
            Type = b.Type,
            Prijs = b.Prijs,
            Afbeelding = b.Afbeelding
        }).ToList();
    }

    public async Task<BoekingDto> MaakBoekingAsync(BoekingDto boekingDto)
    {
        var boeking = new Boeking
        {
            Datum = boekingDto.Datum,
            ContactNaam = boekingDto.ContactNaam,
            ContactAdres = boekingDto.ContactAdres,
            ContactEmail = boekingDto.ContactEmail,
            ContactTelefoonnummer = boekingDto.ContactTelefoonnummer,
            TotaalPrijs = boekingDto.TotaalPrijs,
            Beestjes = boekingDto.Beestjes.Select(b => new Beestje
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToList()
        };

        _context.Boekingen.Add(boeking);
        await _context.SaveChangesAsync();

        return new BoekingDto
        {
            Id = boeking.Id,
            Datum = boeking.Datum,
            ContactNaam = boeking.ContactNaam,
            ContactAdres = boeking.ContactAdres,
            ContactEmail = boeking.ContactEmail,
            ContactTelefoonnummer = boeking.ContactTelefoonnummer,
            TotaalPrijs = boeking.TotaalPrijs,
            Beestjes = boeking.Beestjes.Select(b => new BeestjeDto
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToList()
        };
    }
    
    public async Task<List<Beestje>> GetBeschikbareBeestjesMappedAsync(DateTime selectedDate)
    {
        var beestjesDto = await GetBeschikbareBeestjesAsync(selectedDate);
        return beestjesDto.Select(dto => new Beestje
        {
            Id = dto.Id,
            Naam = dto.Naam,
            Type = dto.Type,
            Prijs = dto.Prijs,
            Afbeelding = dto.Afbeelding
        }).ToList();
    }

    public async Task<BoekingDto> GetBoekingByIdAsync(int id)
    {
        var boeking = await _context.Boekingen
            .Include(b => b.Beestjes)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (boeking == null)
        {
            return null;
        }

        return new BoekingDto
        {
            Id = boeking.Id,
            Datum = boeking.Datum,
            ContactNaam = boeking.ContactNaam,
            ContactAdres = boeking.ContactAdres,
            ContactEmail = boeking.ContactEmail,
            ContactTelefoonnummer = boeking.ContactTelefoonnummer,
            TotaalPrijs = boeking.TotaalPrijs,
            Beestjes = boeking.Beestjes.Select(b => new BeestjeDto
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToList()
        };
    }

    public async Task<List<BoekingDto>> GetAlleBoekingenAsync()
    {
        var boekingen = await _context.Boekingen
            .Include(b => b.Beestjes)
            .ToListAsync();

        return boekingen.Select(b => new BoekingDto
        {
            Id = b.Id,
            Datum = b.Datum,
            ContactNaam = b.ContactNaam,
            ContactAdres = b.ContactAdres,
            ContactEmail = b.ContactEmail,
            ContactTelefoonnummer = b.ContactTelefoonnummer,
            TotaalPrijs = b.TotaalPrijs,
            Beestjes = b.Beestjes.Select(be => new BeestjeDto
            {
                Id = be.Id,
                Naam = be.Naam,
                Type = be.Type,
                Prijs = be.Prijs,
                Afbeelding = be.Afbeelding
            }).ToList()
        }).ToList();
    }

    public async Task VerwijderBoekingAsync(int id)
    {
        var boeking = await _context.Boekingen.FindAsync(id);
        if (boeking != null)
        {
            _context.Boekingen.Remove(boeking);
            await _context.SaveChangesAsync();
        }
    }
}