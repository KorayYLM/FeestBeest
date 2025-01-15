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

        public IEnumerable<UserDto> GetAllUsers()
        {
            var users = _userManager.Users
                .OrderBy(user => user.UserName)
                .ToList();

            var customers = users
                .Where(user => _userManager.IsInRoleAsync(user, "customer").Result)
                .Select(ConvertCustomerDto)
                .ToList();
            return customers;
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

        public async Task<(bool check, string result)> UpdateUser(int id, UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return (false, "User not found");

            user.UserName = userDto.Name;
            user.Email = userDto.Email;
            user.ZipCode = userDto.ZipCode;
            user.HouseNumber = userDto.HouseNumber;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Rank = userDto.Rank;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? (true, "User updated") : (false, "Something went wrong, please try again.");
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