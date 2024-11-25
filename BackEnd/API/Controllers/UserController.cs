using Amazon.Runtime.Internal;
using AutoMapper;
using BackEnd.DTOs.UserDtos;
using BackEnd.DTOs.UserDtos.Requests;
using BackEnd.Utilities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class UserController : BaseController<UserController>
    {

        public UserController(ILogger<UserController> logger, IAuthService authService, IMapper mapper) : base(logger, mapper, authService)
        {
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateUserRequestDto request)
        {
            if (request == null || request.PreviousPayDate?.Date > DateTime.Now)
            {
                return BadRequest();
            }

            string? userId = null;

            //TODO: Handle it better
            try
            {
                userId = User.GetUserId();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error: {ex}", ex);
                BadRequest();
            }

            if (userId == null) return BadRequest();

            ApplicationUser? user = null;

            try
            {
                user = await _authService.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error: {ex}", ex);
                BadRequest();
            }

            if(user == null) return BadRequest();

            user.PreviousPayDate = request.PreviousPayDate;
            user.BiWeeklySalary = request.BiWeeklySalary;

            await _authService.UpdateUserAsync(user);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            string userId = null;

            try
            {
                userId = User.GetUserId();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error: {ex}", ex);
                BadRequest();
            }

            if (userId == null) return BadRequest();

            ApplicationUser? user = null;

            try
            {
                user = await _authService.GetUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error: {ex}", ex);
                BadRequest();
            }

            if (user == null) return BadRequest();

            var userDto = new UserDto() 
            {
                BiweeklyPay = user.BiWeeklySalary,
                LastPayDate = user.PreviousPayDate
            };

            return Ok(userDto);
        }
    }
}
