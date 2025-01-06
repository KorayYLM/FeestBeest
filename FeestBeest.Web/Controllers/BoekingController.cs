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
    public async Task<IActionResult> Index()
    {
        var boekingen = await _boekingService.GetAlleBoekingenAsync();
        var viewModel = boekingen.Select(BoekingViewModel.FromDto).ToList();
        return View(viewModel);
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