using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(string username, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(username);

            if (existingUser != null) throw new UserAlreadyExistsException();

            var appUser = new ApplicationUser
            {
                UserName = username
            };

            return await _userManager.CreateAsync(appUser, password);
        }

        public async Task<string> GetUsernameAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null || appUser.UserName == null) throw new UserDoesNotExistException();

            return appUser.UserName;
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser == null) throw new UserDoesNotExistException();

            return appUser.Id.ToString();
        }

        public async Task<SignInResult> SignInUserAsync(string username, string password)
        {
            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) throw new UserDoesNotExistException();

            return await _signInManager.PasswordSignInAsync(appUser, password, false, false);
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
