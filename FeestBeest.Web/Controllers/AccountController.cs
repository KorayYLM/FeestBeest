using FeestBeest.Data.Dto;
using FeestBeest.Data.Services;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeestBeest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(AccountService accountService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await RedirectToRoleBasedAction(user);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        private async Task<IActionResult> RedirectToRoleBasedAction(User user)
        {
            if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return RedirectToAction("Index", "Order");
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            throw new InvalidOperationException("Invalid account role");
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
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid) return View(accountViewModel);

            var userDto = MapAccountViewModelToUserDto(accountViewModel);
            var (success, message) = await _accountService.CreateUser(userDto);
            if (success)
            {
                return RedirectToAction("ShowPassword", new { password = message });
            }

            ModelState.AddModelError(string.Empty, message);
            return View(accountViewModel);
        }

        private static UserDto MapAccountViewModelToUserDto(AccountViewModel accountViewModel)
        {
            return new UserDto
            {
                Name = accountViewModel.Name,
                Email = accountViewModel.Email,
                Rank = accountViewModel.Rank,
                HouseNumber = accountViewModel.HouseNumber,
                PhoneNumber = accountViewModel.PhoneNumber,
                ZipCode = accountViewModel.ZipCode
            };
        }

        public IActionResult ShowPassword(string password)
        {
            ViewBag.Password = password;
            return View();
        }
    }
}