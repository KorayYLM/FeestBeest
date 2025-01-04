using FeestBeest.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FeestBeest.Data.Services;
using FeestBeest.Web.ViewModels;

public class BeestjeController : Controller
{
    private readonly IBeestjeService _beestjeService;

    public BeestjeController(IBeestjeService beestjeService)
    {
        _beestjeService = beestjeService;
    }

    public async Task<IActionResult> Index()
    {
        var beestjes = await _beestjeService.GetAllBeestjesAsync();
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

        var viewModel = await _beestjeService.GetBeestjeByIdAsync(id.Value);
        if (viewModel == null)
        {
            return NotFound();
        }

        return View(BeestjeViewModel.FromDto(viewModel));   
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