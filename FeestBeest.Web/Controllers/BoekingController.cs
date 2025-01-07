using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;

[Route("boekingen")]
public class BoekingController : Controller
{
    private readonly IBoekingService _boekingService;
    private readonly ILogger<BoekingController> _logger;

    public BoekingController(IBoekingService boekingService, ILogger<BoekingController> logger)
    {
        _boekingService = boekingService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? selectedDate)
    {
        var boekingen = await _boekingService.GetAlleBoekingenAsync();
        var viewModel = boekingen.Select(BoekingViewModel.FromDto).ToList();
        var beestjes = await _boekingService.GetBeschikbareBeestjesMappedAsync(selectedDate ?? DateTime.Now);

        var model = new BoekingIndexViewModel
        {
            Boekingen = viewModel,
            Beestjes = beestjes,
            SelectedDate = selectedDate ?? DateTime.Now
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BoekingIndexViewModel model)
    {
        if (ModelState.IsValid)
        {
            var allBeestjes = await _boekingService.GetBeschikbareBeestjesMappedAsync(model.SelectedDate);
            model.SelectedBeestjes = allBeestjes
                .Where(b => model.SelectedBeestjesIds.Contains(b.Id))
                .ToList();
            model.Beestjes = allBeestjes;

            // Create a new booking in the database
            var boeking = new BoekingDto
            {
                Datum = model.SelectedDate,
                Beestjes = model.SelectedBeestjes.Select(b => new BeestjeDto
                {
                    Id = b.Id,
                    Naam = b.Naam,
                    Type = b.Type,
                    Prijs = b.Prijs,
                    Afbeelding = b.Afbeelding
                }).ToList(),
                IsBevestigd = false
            };
            var createdBoeking = await _boekingService.MaakBoekingAsync(boeking);

            return RedirectToAction("GegevensInvullen", new { id = createdBoeking.Id });
        }
        else
        {
            model.Beestjes = new List<Beestje>();
        }

        return View(model);
    }

   // Gegevens invullen
    [HttpGet("gegevensinvullen/{id}")]
    public async Task<IActionResult> GegevensInvullen(int id)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            _logger.LogWarning("Boeking niet gevonden: {Id}", id);
            return RedirectToAction("Index");
        }

        var model = new GegevensInvullenViewModel
        {
            SelectedDate = boeking.Datum,
            SelectedBeestjes = boeking.Beestjes.Select(b => new Beestje
            {
                Id = b.Id,
                Naam = b.Naam,
                Type = b.Type,
                Prijs = b.Prijs,
                Afbeelding = b.Afbeelding
            }).ToList()
        };

        return View(model);
    }

    [HttpPost("gegevensinvullen/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GegevensInvullen(int id, GegevensInvullenViewModel model)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            _logger.LogWarning("Boeking niet gevonden: {Id}", id);
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            boeking.ContactNaam = model.ContactNaam;
            boeking.ContactAdres = model.ContactAdres;
            boeking.ContactEmail = model.ContactEmail;
            boeking.ContactTelefoonnummer = model.ContactTelefoonnummer;

            await _boekingService.UpdateBoekingAsync(boeking);
            return RedirectToAction("Bevestigen", new { id = boeking.Id });
        }

        model.SelectedDate = boeking.Datum;
        model.SelectedBeestjes = boeking.Beestjes.Select(b => new Beestje
        {
            Id = b.Id,
            Naam = b.Naam,
            Type = b.Type,
            Prijs = b.Prijs,
            Afbeelding = b.Afbeelding
        }).ToList();

        return View(model);
    }

    // Bevestiging
    [HttpGet("bevestigen/{id}")]
    public async Task<IActionResult> Bevestigen(int id)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            _logger.LogWarning("Boeking niet gevonden: {Id}", id);
            return RedirectToAction("Index");
        }

        var model = new BevestigenViewModel
        {
            Boeking = boeking
        };

        return View(model);
    }

    [HttpPost("bevestigen/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmBevestigen(int id)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            _logger.LogWarning("Boeking niet gevonden: {Id}", id);
            return RedirectToAction("Index");
        }

        boeking.IsBevestigd = true;
        await _boekingService.UpdateBoekingAsync(boeking);
        return RedirectToAction("Details", new { id = boeking.Id });
    }

    // Details van een boeking
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            _logger.LogWarning("Boeking niet gevonden: {Id}", id);
            return NotFound();
        }

        var viewModel = BoekingViewModel.FromDto(boeking);
        return View(viewModel);
    }
}