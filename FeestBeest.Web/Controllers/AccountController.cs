using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;
using FeestBeest.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, UserManager<Account> userManager, SignInManager<Account> signInManager, RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
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
        return View();
    }

[HttpPost]
public async Task<IActionResult> Create(AccountViewModel model)
{
    _logger.LogInformation("Create method called with model: {@Model}", model);

    if (ModelState.IsValid)
    {
        _logger.LogInformation("Model state is valid.");

        if (!Enum.TryParse<KlantenkaartType>(model.KlantType, out var klantType))
        {
            _logger.LogWarning("Invalid KlantType: {KlantType}", model.KlantType);
            ModelState.AddModelError(string.Empty, "Ongeldig klanttype.");
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", model.Email);
            ModelState.AddModelError(string.Empty, "Een gebruiker met dit emailadres bestaat al.");
            return View(model);
        }

        var user = new Account
        {
            UserName = model.Email,
            Email = model.Email,
            Naam = model.Naam,
            Adres = model.Adres,
            Telefoonnummer = model.Telefoonnummer,
            KlantType = klantType
        };

        _logger.LogInformation("Creating user: {@User}", user);

        var password = GeneratePassword();
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created successfully.");

            var klantTypeString = klantType.ToString();

            if (!await _roleManager.RoleExistsAsync(klantTypeString))
            {
                _logger.LogInformation("Role {Role} does not exist. Creating role.", klantTypeString);
                await _roleManager.CreateAsync(new IdentityRole(klantTypeString));
            }

            await _userManager.AddToRoleAsync(user, klantTypeString);
            await _accountService.CreateAccount(user);

            _logger.LogInformation("User added to role and account service called.");

            return RedirectToAction("ShowPassword", new { email = user.Email, password });
        }

        _logger.LogWarning("User creation failed.");
        foreach (var error in result.Errors)
        {
            _logger.LogWarning("Error: {Error}", error.Description);
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
    else
    {
        _logger.LogWarning("Model state is invalid.");
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                _logger.LogWarning("Property: {Property}, Error: {Error}", state.Key, error.ErrorMessage);
            }
        }
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
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked out.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Login is not allowed.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    private string GeneratePassword()
    {
        return "GeneratedPassword123!";
    }
}