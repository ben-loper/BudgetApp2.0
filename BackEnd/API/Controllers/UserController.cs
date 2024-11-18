using BackEnd.DTOs.UserDtos;
using BackEnd.Utilities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly IAuthService _authService;

        public UserController(ILogger<UserController> logger, IAuthService authService)
            : base(logger) 
        {
            _authService = authService;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateUserRequestDto request)
        {
            if (request == null || request.PreviousPayDate?.Date > DateTime.Now)
            {
                return BadRequest();
            }

            string userId = null;

            //TODO: Handle it better
            try
            {
                userId = User.GetUserId();
            }
            catch (Exception ex)
            {
                BadRequest();
            }

            ApplicationUser user = null;

            try
            {
                user = await _authService.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                BadRequest();
            }

            user.PreviousPayDate = request.PreviousPayDate;
            user.BiWeeklySalary = request.BiWeeklySalary;

            await _authService.UpdateUserAsync(user);

            return Ok();
        }
    }
}
