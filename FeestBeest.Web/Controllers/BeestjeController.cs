using FeestBeest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BeestjeController : Controller
{
    private readonly FeestBeestContext _context;

    public BeestjeController(FeestBeestContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var beestjes = await _context.Beestjes
            .Select(b => new BeestjeViewModel
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToListAsync();
        return View(beestjes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BeestjeViewModel viewModel)
    {
        if (ModelState.IsValid)
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
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var beestje = await _context.Beestjes.FindAsync(id);
        if (beestje == null)
        {
            return NotFound();
        }

        var viewModel = new BeestjeViewModel
        {
            Id = beestje.Id,
            Naam = beestje.Naam,
            Type = beestje.Type,
            Prijs = beestje.Prijs,
            Afbeelding = beestje.Afbeelding
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BeestjeViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var beestje = await _context.Beestjes.FindAsync(id);
                beestje.Naam = viewModel.Naam;
                beestje.Type = viewModel.Type;
                beestje.Prijs = viewModel.Prijs;
                beestje.Afbeelding = viewModel.Afbeelding;

                _context.Update(beestje);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeestjeExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var beestje = await _context.Beestjes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (beestje == null)
        {
            return NotFound();
        }

        var viewModel = new BeestjeViewModel
        {
            Id = beestje.Id,
            Naam = beestje.Naam,
            Type = beestje.Type,
            Prijs = beestje.Prijs,
            Afbeelding = beestje.Afbeelding
        };
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var beestje = await _context.Beestjes.FindAsync(id);
        _context.Beestjes.Remove(beestje);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BeestjeExists(int id)
    {
        return _context.Beestjes.Any(e => e.Id == id);
    }
}