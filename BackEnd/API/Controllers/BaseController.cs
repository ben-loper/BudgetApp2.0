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


    }
}
