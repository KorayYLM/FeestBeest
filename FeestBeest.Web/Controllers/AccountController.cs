using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;
using FeestBeest.Services;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountService accountService,
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        RoleManager<IdentityRole<int>> roleManager,
        ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Create()
    {
        _logger.LogInformation("Navigated to Create view.");
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
        _logger.LogInformation("Navigated to ShowPassword view for email: {Email}", email);
        var model = new ShowPasswordViewModel
        {
            Email = email,
            Password = password
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        _logger.LogInformation("Navigated to Login view.");
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("User logged out.");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}