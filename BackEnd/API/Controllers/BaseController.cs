using BackEnd.Exceptions;
using Domain.Models;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseController<T>(ILogger<T> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : ControllerBase
    {
        protected readonly ILogger<T> _logger = logger;
        protected readonly UserManager<ApplicationUser> _userManager = userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        protected string GetUserId() => _userManager.GetUserId(User) ?? throw new UserNotLoggedInException();

        protected async Task<string> GetUsernameAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotLoggedInException();

            return user.UserName ?? throw new UserNotLoggedInException();
        }

        protected async Task<string> GetUserIdByUsername(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) throw new UserDoesNotExistException();

            return user.Id.ToString();
        }
    }
}
