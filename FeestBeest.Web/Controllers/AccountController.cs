﻿using FeestBeest.Data.Services;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeestBeest.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(AccountService accountService, SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _userManager = userManager;
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
    }
}
    
    
    
