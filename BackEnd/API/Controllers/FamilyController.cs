using BackEnd.DTOs.FamilyDtos;
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
        private readonly IAuthService _authService;

        public FamilyController(ILogger<FamilyController> logger, IFamilyService familyService, IAuthService authService)
            : base(logger)
        {
            _familyService = familyService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<FamilyDto>> CreateFamily(CreateFamilyRequestDto createFamilyDto)
        {
            if (createFamilyDto == null) return BadRequest();

            var family = new Family()
            {
                Name = createFamilyDto.Name,
                AdminUserIds = [],
                MemberUserIds = []
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

            var familyDto = new FamilyDto()
            {
                Id = family.Id.ToString(),
                Name = family.Name,
                AdminUsers = family.AdminUserIds,
                Members = family.MemberUserIds
            };

            return familyDto;
        }

        [HttpGet]
        public async Task<ActionResult<FamilyDto>> GetFamily()
        {
            var family = await _familyService.GetFamilyAsync(User.GetUserId());

            if (family == null || family.Id == null) return NotFound();

            var familyDto = new FamilyDto()
            {
                Id = family.Id,
                Name = family.Name,
                AdminUsers = [],
                Members = []
            };

            foreach (var userId in family.AdminUserIds)
            {
                var username = await _authService.GetUsernameAsync(userId);

                familyDto.AdminUsers.Add(username);
            }

            foreach (var userId in family.MemberUserIds)
            {
                var username = await _authService.GetUsernameAsync(userId);

                familyDto.Members.Add(username);
            }

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
                family = await _familyService.GetFamilyAsync(User.GetUserId());
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

            var familyDto = new FamilyDto()
            {
                Id = family.Id,
                Name = updatedFamily.Name,
                AdminUsers = [],
                Members = []
            };

            foreach (var adminUserId in updatedFamily.AdminUserIds)
            {
                var id = await _authService.GetUsernameAsync(adminUserId);

                familyDto.AdminUsers.Add(id);
            }

            foreach (var adminUserId in updatedFamily.MemberUserIds)
            {
                var id = await _authService.GetUsernameAsync(adminUserId);

                familyDto.Members.Add(id);
            }

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
                family = await _familyService.GetFamilyAsync(User.GetUserId());
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

            var familyDto = new FamilyDto()
            {
                Id = family.Id,
                Name = updatedFamily.Name,
                AdminUsers = [],
                Members = []
            };

            foreach (var adminUserId in updatedFamily.AdminUserIds)
            {
                var id = await _authService.GetUsernameAsync(adminUserId);

                familyDto.AdminUsers.Add(id);
            }

            foreach (var adminUserId in updatedFamily.MemberUserIds)
            {
                var id = await _authService.GetUsernameAsync(adminUserId);

                familyDto.Members.Add(id);
            }

            return familyDto;
        }
    }
}
