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

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
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