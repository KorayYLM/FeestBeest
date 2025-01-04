using System.Security.Cryptography;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FeestBeest.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly FeestBeestContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(FeestBeestContext context, UserManager<Account> userManager, ILogger<AccountService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> CreateAccount(Account account)
        {
            if (await _userManager.FindByEmailAsync(account.Email) != null)
            {
                throw new Exception("Email already in use");
            }

            var password = PasswordGenerator();
            var result = await _userManager.CreateAsync(account, password);
            if (!result.Succeeded)
            {
                throw new Exception("Something went wrong, please try again.");
            }

            await _userManager.AddToRoleAsync(account, "User");
            _logger.LogInformation("Account created successfully with email: {Email}", account.Email);
            return password;
        }

        // public async Task<SignInResult> Login(string email, string password, bool rememberMe)
        // {
        //     var user = await _userManager.FindByEmailAsync(email);
        //     if (user == null)
        //     {
        //         throw new Exception("Invalid login attempt.");
        //     }
        //
        //     var passwordHasher = new PasswordHasher<Account>();
        //     var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        //     if (verificationResult == PasswordVerificationResult.Failed)
        //     {
        //         throw new Exception("Invalid login attempt.");
        //     }
        //
        //     _logger.LogInformation("User logged in with email: {Email}", email);
        //     return SignInResult.Success;
        // }
        
        private string PasswordGenerator()
        {
            const int length = 12;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";

            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);
                var password = new char[length];
                for (int i = 0; i < length; i++)
                {
                    password[i] = validChars[buffer[i] % validChars.Length];
                }
                return new string(password);
            }
        }
    }
}