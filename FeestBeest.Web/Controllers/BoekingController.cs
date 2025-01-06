using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;

[Route("boekingen")]
public class BoekingController : Controller
{
    private readonly IBoekingService _boekingService;

    public BoekingController(IBoekingService boekingService)
    {
        _boekingService = boekingService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? selectedDate)
    {
        // Get all bookings
        var boekingen = await _boekingService.GetAlleBoekingenAsync();
        var viewModel = boekingen.Select(BoekingViewModel.FromDto).ToList();

        // Get all available beestjes for the selected date
        var beestjesDto = await _boekingService.GetBeschikbareBeestjesAsync(selectedDate ?? DateTime.Now);
        var beestjes = beestjesDto.Select(dto => new Beestje
        {
            Id = dto.Id,
            Naam = dto.Naam,
            Type = dto.Type,
            Prijs = dto.Prijs,
            Afbeelding = dto.Afbeelding
        }).ToList();

        // Create the view model
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
            // Process the selected beestjes
            var selectedBeestjes = model.Beestjes.Where(b => model.SelectedBeestjes.Any(sb => sb.Id == b.Id)).ToList();
            model.SelectedBeestjes = selectedBeestjes;

            // Continue with your logic
        }

        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new BoekingViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BoekingViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            await _boekingService.MaakBoekingAsync(viewModel.ToDto());
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
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