using BackEnd.DTOs;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Create(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var appUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Choose an available username");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var appUser = await _userManager.FindByEmailAsync(user.Email);

            if (appUser == null) return BadRequest("Email or password is incorrect");

            var result = await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);

            if (result.Succeeded)
            {
                
                return Ok();
            }
            else
            {
                return BadRequest("Email or password is incorrect");
            }
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
