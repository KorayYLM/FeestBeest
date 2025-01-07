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

            // Store the booking ID in TempData
            TempData["BoekingId"] = createdBoeking.Id;

            return RedirectToAction("GegevensInvullen");
        }
        else
        {
            model.Beestjes = new List<Beestje>();
        }

        return View(model);
    }

    [HttpGet("gegevensinvullen")]
    public async Task<IActionResult> GegevensInvullen()
    {
        // Haal data uit TempData
        var boekingId = TempData["BoekingId"] as int?;
        if (!boekingId.HasValue)
        {
            return RedirectToAction("Index"); // Terug naar stap 1 als data ontbreekt
        }

        var boeking = await _boekingService.GetBoekingByIdAsync(boekingId.Value);
        if (boeking == null)
        {
            return RedirectToAction("Index");
        }

        // Maak het model aan
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

    [HttpPost("gegevensinvullen")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GegevensInvullen(GegevensInvullenViewModel model)
    {
        if (ModelState.IsValid)
        {
            var boekingId = TempData["BoekingId"] as int?;
            if (!boekingId.HasValue)
            {
                return RedirectToAction("Index");
            }

            var boeking = await _boekingService.GetBoekingByIdAsync(boekingId.Value);
            if (boeking == null)
            {
                return RedirectToAction("Index");
            }

            // Update the booking with additional information
            boeking.ContactNaam = model.ContactNaam;
            boeking.ContactAdres = model.ContactAdres;
            boeking.ContactEmail = model.ContactEmail;
            boeking.ContactTelefoonnummer = model.ContactTelefoonnummer;

            await _boekingService.UpdateBoekingAsync(boeking);

            return RedirectToAction("Bevestigen");
        }

        return View(model);
    }

    [HttpGet("bevestigen")]
    public async Task<IActionResult> Bevestigen()
    {
        var boekingId = TempData["BoekingId"] as int?;
        if (!boekingId.HasValue)
        {
            return RedirectToAction("Index");
        }

        var boeking = await _boekingService.GetBoekingByIdAsync(boekingId.Value);
        if (boeking == null)
        {
            return RedirectToAction("Index");
        }

        // Maak het model aan
        var model = new BevestigenViewModel
        {
            Boeking = boeking
        };

        return View(model);
    }

    [HttpPost("bevestigen")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Bevestigen(BevestigenViewModel model)
    {
        var boekingId = TempData["BoekingId"] as int?;
        if (!boekingId.HasValue)
        {
            return RedirectToAction("Index");
        }

        var boeking = await _boekingService.GetBoekingByIdAsync(boekingId.Value);
        if (boeking == null)
        {
            return RedirectToAction("Index");
        }

        // Mark the booking as confirmed
        boeking.IsBevestigd = true;
        await _boekingService.UpdateBoekingAsync(boeking);

        return RedirectToAction("Details", new { id = boeking.Id });
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new BoekingViewModel());
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var boeking = await _boekingService.GetBoekingByIdAsync(id);
        if (boeking == null)
        {
            return NotFound();
        }

        var viewModel = BoekingViewModel.FromDto(boeking);
        return View(viewModel);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _boekingService.VerwijderBoekingAsync(id);
        return RedirectToAction(nameof(Index));
    }
}