using System.Threading.Tasks;
using FeestBeest.Data.Models;

namespace FeestBeest.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccount(Account account);
        Task<Account> GetAccountById(string id);
        Task<Account> UpdateAccount(Account account);
        Task DeleteAccount(string id);
    }
}