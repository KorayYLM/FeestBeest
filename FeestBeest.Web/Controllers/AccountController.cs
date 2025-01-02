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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountService accountService,
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        RoleManager<IdentityRole> roleManager,
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
    _logger.LogInformation("Create method called with model: {@Model}", model);

    // Validatie van de modelstatus
    if (!ModelState.IsValid)
    {
        _logger.LogWarning("Model state is invalid.");
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                _logger.LogWarning("Property: {Property}, Error: {Error}", state.Key, error.ErrorMessage);
            }
        }
        return View(model);
    }

    // Klanttype validatie
    if (!Enum.TryParse<KlantenkaartType>(model.KlantType, out var klantType))
    {
        _logger.LogWarning("Invalid KlantType: {KlantType}", model.KlantType);
        ModelState.AddModelError(string.Empty, "Ongeldig klanttype.");
        return View(model);
    }

    // Controle of de gebruiker al bestaat op basis van het emailadres
    var existingUser = await _userManager.FindByEmailAsync(model.Email);
    if (existingUser != null)
    {
        _logger.LogWarning("User with email {Email} already exists.", model.Email);
        ModelState.AddModelError(string.Empty, "Een gebruiker met dit emailadres bestaat al.");
        return View(model);
    }

    // Genereer een nieuwe unieke ID voor de gebruiker (handmatig)
    var newId = await GenerateUniqueId();

    var user = new Account
    {
        Id = newId,  // Handmatig gegenereerde ID
        UserName = model.Email,
        Email = model.Email,
        Naam = model.Naam,
        Adres = model.Adres,
        Telefoonnummer = model.Telefoonnummer,
        KlantType = klantType
    };

    _logger.LogInformation("Creating user: {@User}", user);

    // Genereer een wachtwoord voor de gebruiker
    var password = GeneratePassword();
    var result = await _userManager.CreateAsync(user, password);

    // Foutafhandeling bij het aanmaken van de gebruiker
    if (!result.Succeeded)
    {
        _logger.LogWarning("User creation failed.");
        foreach (var error in result.Errors)
        {
            _logger.LogWarning("Error: {Error}", error.Description);
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    _logger.LogInformation("User created successfully.");

    // Controleer of de rol bestaat, zo niet, maak deze aan
    var klantTypeString = klantType.ToString();
    if (!await _roleManager.RoleExistsAsync(klantTypeString))
    {
        _logger.LogInformation("Role {Role} does not exist. Creating role.", klantTypeString);
        await _roleManager.CreateAsync(new IdentityRole(klantTypeString));
    }

    _logger.LogInformation("Adding user to role {Role}.", klantTypeString);
    await _userManager.AddToRoleAsync(user, klantTypeString);

    // Roep de accountservice aan om het account te maken
    _logger.LogInformation("Calling account service to create account.");
    await _accountService.CreateAccount(user);

    _logger.LogInformation("User added to role {Role} and account service called.", klantTypeString);

    // Redirect naar de actie voor het tonen van het wachtwoord
    return RedirectToAction("ShowPassword", new { email = user.Email, password });
}

private async Task<string> GenerateUniqueId()
{
    var users = await _userManager.Users.ToListAsync();
    var maxId = users.Any() ? users.Max(u => int.TryParse(u.Id, out var id) ? id : 0) : 0;
    
    _logger.LogInformation("Max existing ID: {MaxId}", maxId);

    var newId = (maxId + 1).ToString();
    
    while (users.Any(u => u.Id == newId))
    {
        _logger.LogInformation("ID {NewId} already exists. Incrementing.", newId);
        maxId++;
        newId = (maxId + 1).ToString();
    }

    _logger.LogInformation("Generated unique ID: {NewId}", newId);

    return newId;
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

    private string GeneratePassword()
    {
        _logger.LogInformation("Generating password.");
        return "GeneratedPassword123!";
    }
}
