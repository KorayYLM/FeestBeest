using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FeestBeest.Data.Models;
using FeestBeest.Data.Services;
using FeestBeest.Web.Models;

public class AccountController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<Account> signInManager, UserManager<Account> userManager, IAccountService accountService, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountViewModel model)
    {
        if (!Enum.TryParse<KlantenkaartType>(model.KlantType, out var klantType))
        {
            ModelState.AddModelError(string.Empty, "Ongeldig klanttype.");
            return View(model);
        }

        var user = new Account
        {
            UserName = model.Email,
            Email = model.Email,
            Naam = model.Naam,
            Adres = model.Adres,
            Telefoonnummer = model.Telefoonnummer,
            KlantType = klantType,
        };

        try
        {
            var password = await _accountService.CreateAccount(user);
            ViewBag.NewAccountAlert = $"Een account is aangemaakt. \nDit zijn de gegevens:\nEmail: {model.Email}\nWachtwoord: {password}";
            return RedirectToAction("ShowPassword", new { email = model.Email, password });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the account.");
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ShowPassword(string email, string password)
    {
        var model = new ShowPasswordViewModel
        {
            Email = email,
            Password = password
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Index", "Home");
    }
}