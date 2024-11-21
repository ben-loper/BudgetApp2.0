using AutoMapper;
using BackEnd.DTOs.UserDtos.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace BackEnd.Controllers
{
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IMapper mapper) : base(logger, mapper)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Create(CreateUserRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _authService.CreateUserAsync(user.Username, user.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Unable to create user due to the following errors: {errors}", string.Join(",", result.Errors));

                return BadRequest("Choose an available username");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(CreateUserRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentitySignInResult result;

            try
            {
                result = await _authService.SignInUserAsync(user.Username, user.Password);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Unable to sign in user - {ex}", ex);
                return BadRequest("Email or password is incorrect");
            }


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
            await _authService.SignOutUserAsync();

            return Ok();
        }
    }
}
