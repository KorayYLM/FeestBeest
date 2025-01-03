using FeestBeest.Data;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FeestBeest.Services
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

        public async Task<Account> CreateAccount(Account account)
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
            return account;
        }

        public async Task<Account> GetAccountById(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task DeleteAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        private string PasswordGenerator()
        {
            // Implement a simple password generator
            return "GeneratedPassword123!";
        }
    }
}