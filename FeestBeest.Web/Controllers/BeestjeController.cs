
using Microsoft.AspNetCore.Mvc;
using FeestBeest.Data.Services;
using FeestBeest.Web.ViewModels;
using FeestBeest.Web.Models;

public class BeestjeController : Controller
{
    private readonly IBeestjeService _beestjeService;

    public BeestjeController(IBeestjeService beestjeService)
    {
        _beestjeService = beestjeService;
    }

    public async Task<IActionResult> Index()
    {
        var beestjesDto = await _beestjeService.GetAllBeestjesAsync();
        var viewModel = new IndexViewModel
        {
            Beestjes = beestjesDto.Select(BeestjeViewModel.FromDto)
        };
        return View(viewModel);
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
            await _beestjeService.CreateBeestjeAsync(viewModel.ToDto());
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

        var beestjeDto = await _beestjeService.GetBeestjeByIdAsync(id.Value);
        if (beestjeDto == null)
        {
            return NotFound();
        }

        var viewModel = BeestjeViewModel.FromDto(beestjeDto);
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

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        await _beestjeService.UpdateBeestjeAsync(viewModel.ToDto());
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _beestjeService.DeleteBeestjeAsync(id);
        return Ok(); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _beestjeService.DeleteBeestjeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}