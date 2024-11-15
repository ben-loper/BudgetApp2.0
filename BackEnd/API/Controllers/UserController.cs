using BackEnd.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    // TODO: Move repo related functions to repo class
    public class UserController : BaseController<UserController>
    {
        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            : base(logger, userManager, signInManager)
        {
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
