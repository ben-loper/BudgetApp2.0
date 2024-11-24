using AutoMapper;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.Utilities;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseController<T>(ILogger<T> logger, IMapper mapper, IAuthService authService) : ControllerBase
    {
        protected readonly ILogger<T> _logger = logger;
        protected readonly IMapper _mapper = mapper;
        protected readonly IAuthService _authService = authService;

        internal async Task<FamilyDto> MapFamilyToDto(Family family)
        {
            var familyDto = _mapper.Map<FamilyDto>(family);

            //TODO: Abstract this to a service or method
            foreach (var userId in family.AdminUserIds)
            {
                var user = await _authService.GetUserAsync(userId);
                familyDto.AdminUsers.Add(user.GetUsername());

                if (familyDto.Budget != null) familyDto.Budget.PayThisMonth += user.TotalPayThisMonth();
            }

            foreach (var userId in family.MemberUserIds)
            {
                var user = await _authService.GetUserAsync(userId);
                familyDto.Members.Add(user.GetUsername());

                if (familyDto.Budget != null) familyDto.Budget.PayThisMonth += user.TotalPayThisMonth();
            }

            return familyDto;
        }
    }
}
