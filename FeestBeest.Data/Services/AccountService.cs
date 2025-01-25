using System.Text;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FeestBeest.Data.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        
        private UserDto ConvertCustomerDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Rank = user.Rank,
                HouseNumber = user.HouseNumber,
                ZipCode = user.ZipCode,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<(bool, string)> CreateUser(UserDto user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return (false, "Email already in use");
            }

            var newUser = new User
            {
                UserName = user.Name,
                Email = user.Email,
                Rank = user.Rank,
                HouseNumber = user.HouseNumber,
                PhoneNumber = user.PhoneNumber,
                ZipCode = user.ZipCode
            };

            var password = PasswordGenerator();
            var result = await _userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
            {
                return (false, "Something went wrong, please try again.");
            }

            await _userManager.AddToRoleAsync(newUser, "customer");
            return (true, password);
        }

        private static string PasswordGenerator()
        {
            var password = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < 8; i++)
            {
                password.Append((char)random.Next(33, 126));
            }

            return password.ToString();
        }

        public UserDto GetUserById(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            return ConvertCustomerDto(user!);
        }
        
    }
}