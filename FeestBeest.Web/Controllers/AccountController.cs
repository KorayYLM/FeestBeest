using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FeestBeest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService accountService, SignInManager<User> signInManager,
            UserManager<User> userManager, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login/check")]
        public async Task<IActionResult> LoginCheck(string loginInput, string password)
        {
            var user = await _userManager.FindByEmailAsync(loginInput);
            if (user != null)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(user, password, isPersistent: false,
                        lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        return RedirectToAction("Index", "Order");
                    }

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "User");
                    }

                    throw new InvalidOperationException("Invalid account role");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        // Add the Create action methods
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    Name = accountViewModel.Name,
                    Email = accountViewModel.Email,
                    Rank = accountViewModel.Rank,
                    HouseNumber = accountViewModel.HouseNumber,
                    PhoneNumber = accountViewModel.PhoneNumber,
                    ZipCode = accountViewModel.ZipCode
                };
                var (success, message) = await _accountService.CreateUser(userDto);
                if (success)
                {
                    // Redirect to ShowPassword view with the generated password
                    return RedirectToAction("ShowPassword", new { password = message });
                }
                ModelState.AddModelError(string.Empty, message);
            }
            return View(accountViewModel);
        }
        
        public IActionResult ShowPassword(string password)
        {
            ViewBag.Password = password;
            return View();
        }
    }
    
    
}