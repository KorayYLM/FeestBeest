using System.Threading.Tasks;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FeestBeest.Data.Services
{
    public interface IAccountService
    {
        Task<string> CreateAccount(Account account);
        // Task<SignInResult> Login(string email, string password, bool rememberMe);
        
    }
}