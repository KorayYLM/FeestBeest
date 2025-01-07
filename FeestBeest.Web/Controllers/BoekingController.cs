using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        // Get all bookings
        var boekingen = await _boekingService.GetAlleBoekingenAsync();
        var viewModel = boekingen.Select(BoekingViewModel.FromDto).ToList();

        // Get all available beestjes for the selected date
        var beestjes = await _boekingService.GetBeschikbareBeestjesMappedAsync(selectedDate ?? DateTime.Now);

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
            // Get all available beestjes for the selected date
            var allBeestjes = await _boekingService.GetBeschikbareBeestjesMappedAsync(model.SelectedDate);

            // Map selected IDs to Beestjes
            model.SelectedBeestjes = allBeestjes
                .Where(b => model.SelectedBeestjesIds.Contains(b.Id))
                .ToList();

            // Update the model with all beestjes
            model.Beestjes = allBeestjes;
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