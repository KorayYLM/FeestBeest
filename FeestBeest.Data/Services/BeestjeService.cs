using FeestBeest.Data;
using Microsoft.EntityFrameworkCore;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;

public class BeestjeService : IBeestjeService
{
    private readonly FeestBeestContext _context;

    public BeestjeService(FeestBeestContext context)
    {
        _context = context;
    }

    public async Task<List<BeestjeDto>> GetAllBeestjesAsync()
    {
        return await _context.Beestjes
            .Select(b => new BeestjeDto
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToListAsync();
    }

    public async Task<BeestjeDto> GetBeestjeByIdAsync(int id)
    {
        var beestje = await _context.Beestjes.FindAsync(id);
        if (beestje == null)
        {
            return null;
        }

        return new BeestjeDto
        {
            Id = beestje.Id,
            Naam = beestje.Naam,
            Type = beestje.Type,
            Prijs = beestje.Prijs,
            Afbeelding = beestje.Afbeelding
        };
    }

    public async Task CreateBeestjeAsync(BeestjeDto viewModel)
    {
        var beestje = new Beestje
        {
            Naam = viewModel.Naam,
            Type = viewModel.Type,
            Prijs = viewModel.Prijs,
            Afbeelding = viewModel.Afbeelding
        };
        _context.Add(beestje);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBeestjeAsync(BeestjeDto viewModel)
    {
        var beestje = await _context.Beestjes.FindAsync(viewModel.Id);
        if (beestje == null)
        {
            return;
        }

        beestje.Naam = viewModel.Naam;
        beestje.Type = viewModel.Type;
        beestje.Prijs = viewModel.Prijs;
        beestje.Afbeelding = viewModel.Afbeelding;

        _context.Update(beestje);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBeestjeAsync(int id)
    {
        var beestje = await _context.Beestjes.FindAsync(id);
        if (beestje == null)
        {
            return;
        }

        _context.Beestjes.Remove(beestje);
        await _context.SaveChangesAsync();
    }

    public bool BeestjeExists(int id)
    {
        return _context.Beestjes.Any(e => e.Id == id);
    }
}