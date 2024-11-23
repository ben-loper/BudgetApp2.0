using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Services
{
    public interface IAuthService
    {
        Task<ApplicationUser> GetUserAsync(string userId);
        Task<IdentityResult> CreateUserAsync(string username, string password);
        Task<SignInResult> SignInUserAsync(string username, string password);
        Task<string> GetUserIdAsync(string userName);
        Task SignOutUserAsync();
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    }
}
