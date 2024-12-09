using FeestBeest.Data;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly FeestBeestContext _context;

    public AccountController(FeestBeestContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create(Account account)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         account.Wachtwoord = GeneratePassword();
    //         _context.Add(account);
    //         await _context.SaveChangesAsync();
    //         ViewBag.Wachtwoord = account.Wachtwoord;
    //         return View("ShowPassword", account);
    //     }
    //     return View(account);
    // }
    //
    // private string GeneratePassword()
    // {
    //     return "GeneratedPassword123";
    // }
}