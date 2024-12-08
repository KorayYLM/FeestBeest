using FeestBeest.Data;
using System.Threading.Tasks;
using FeestBeest.Data.Models;

namespace FeestBeest.Services
{
    public class AccountService : IAccountService
    {
        private readonly FeestBeestContext _context;

        public AccountService(FeestBeestContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
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
    }
}