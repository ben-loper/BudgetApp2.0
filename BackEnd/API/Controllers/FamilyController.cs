using AutoMapper;
using BackEnd.DTOs.BudgetDtos;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.DTOs.FamilyDtos.Requests;
using BackEnd.Utilities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class FamilyController : BaseController<FamilyController>
    {
        private readonly IFamilyService _familyService;

        public FamilyController(ILogger<FamilyController> logger, IFamilyService familyService, IAuthService authService, IMapper mapper) : base(logger, mapper, authService)
        {
            _familyService = familyService;
        }

        [HttpPost]
        public async Task<ActionResult<FamilyDto>> CreateFamily(CreateFamilyRequestDto createFamilyDto)
        {
            if (createFamilyDto == null) return BadRequest();

            var family = new Family()
            {
                Name = createFamilyDto.Name,
                AdminUserIds = [],
                MemberUserIds = [],
                Budget = new Budget()
                {
                    MonthlyBills = [],
                    BudgetCategories = []
                }
            };

            try
            {
                family = await _familyService.CreateFamilyAsync(family, User.GetUserId());
            }
            catch (UserAlreadyInFamilyException)
            {
                _logger.LogError("Unable to create Family due to user already being in a family");

                return BadRequest("Unable to create Family. User is already in a family");

            }
            catch(Exception ex)
            {
                _logger.LogError("Unable to create Family: {ex}", ex);
                return BadRequest();
            }

            if (family == null || family.Id == null)
            {
                return BadRequest();
            }

            var familyDto = _mapper.Map<FamilyDto>(family);

            foreach (var userId in family.AdminUserIds)
            {
                var user = await _authService.GetUserAsync(userId);
                familyDto.AdminUsers.Add(user.GetUsername());

                if (familyDto.Budget != null) familyDto.Budget.PayThisMonth += user.TotalPayThisMonth();
            }

            return familyDto;
        }

        // TODO: May make more sense to break out into a budget service
        [HttpGet]
        public async Task<ActionResult<FamilyDto>> GetFamily()
        {
            var family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());

            if (family == null || family.Id == null) return NotFound();

            var familyDto = await MapFamilyToDto(family);

            return Ok(familyDto);
        }

        // TODO: Add ability to switch a user from admin to member
        // TODO: Setup so the user must be an admin for the family to add/remove users
        //TODO: Can I make this more RESTful?
        [HttpPut("AddUserToFamily")]
        public async Task<ActionResult<FamilyDto>> AddUserToFamily(AddUserToFamilyRequestDto request)
        {
            string? userId = null;

            try
            {
                userId = await _authService.GetUserIdAsync(request.UserName);
            }
            catch (UserDoesNotExistException)
            {
                return BadRequest("User does not exist");
            }

            Family family;

            try
            {
                family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while trying to retrieve family for user: {ex}", ex);

                return NotFound();
            }
            

            if (family == null || family.Id == null)
            {
                return NotFound("User is not part of a family");
            }

            Family updatedFamily;

            try
            {
                updatedFamily = await _familyService.AddUserToFamilyAsync(family.Id, userId, request.Role);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Exception occurred while trying to add user to family: {ex}", ex);
                
                // TODO: Need to handle this more elegantly
                return BadRequest();
            }

            var familyDto = await MapFamilyToDto(family);

            return familyDto;
        }

        //TODO: Can I make this more RESTful?
        // TODO: Add Authorization to ensure user is an admin for the family
        [HttpPut("RemoveUserFromFamily")]
        public async Task<ActionResult<FamilyDto>> RemoveUserFromFamily(RemoveUserFromFamilyRequestDto request)
        {
            string? userId = null;

            try
            {
                userId = await _authService.GetUserIdAsync(request.UserName);
            }
            catch (UserDoesNotExistException)
            {
                return BadRequest("User does not exist");
            }

            Family family;

            try
            {
                family = await _familyService.GetFamilyByUserIdAsync(User.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while trying to retrieve family for user: {ex}", ex);

                return NotFound();
            }


            if (family == null || family.Id == null)
            {
                return NotFound("User is not part of a family");
            }

            Family updatedFamily;

            try
            {
                updatedFamily = await _familyService.RemoveUserFromFamilyAsync(family.Id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred while trying to add user to family: {ex}", ex);

                // TODO: Need to handle this more elegantly
                return BadRequest();
            }

            var familyDto = await MapFamilyToDto(family);

            return familyDto;
        }
    }
}
