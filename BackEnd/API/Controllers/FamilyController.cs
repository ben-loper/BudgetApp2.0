using AspNetCore.Identity.Mongo.Mongo;
using BackEnd.DTOs.FamilyDtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BackEnd.Controllers
{
    public class FamilyController : BaseController<FamilyController>
    {
        private readonly IMongoCollection<Family> _familyCollection;

        public FamilyController(ILogger<FamilyController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, MongoDbContext context) 
            : base(logger, userManager, signInManager)
        {
            _familyCollection = context.Families;
        }

        [HttpPost]
        public async Task<ActionResult<FamilyDto>> CreateFamily(CreateFamilyRequestDto createFamilyDto) 
        {
            if (createFamilyDto == null) return BadRequest();

            var existingFamily = await _familyCollection.FirstOrDefaultAsync(f => f.AdminUserIds.Contains(GetUserId())
                                                                || f.MemberUserIds.Contains(GetUserId())
                                                            );

            if (existingFamily != null) return BadRequest("User is already in a family");

            var family = new Family()
            {
                Name = createFamilyDto.Name,
                AdminUserIds = new List<string>()
                {
                    GetUserId()
                },
                MemberUserIds = new List<string>()
            };

            await _familyCollection.InsertOneAsync(family);
            
            var familyDto = new FamilyDto()
            {
                Name = createFamilyDto.Name,
                AdminUsers = [await GetUsernameAsync()],
                Members = new List<string>()
            };

            return familyDto;
        }

        [HttpGet]
        public async Task<ActionResult<FamilyDto>> GetFamily()
        {
            var family = await _familyCollection.FirstOrDefaultAsync(f => f.AdminUserIds.Contains(GetUserId())
                                                                || f.MemberUserIds.Contains(GetUserId())
                                                            );

            if (family == null) return NotFound();

            var familyDto = new FamilyDto()
            {
                Name = family.Name,
                AdminUsers = [],
                Members = []
            };

            var adminUsers = new List<string>();

            foreach (var userId in family.AdminUserIds)
            {
                var person = await _userManager.FindByIdAsync(userId);

                adminUsers.Add(person.UserName);
            }

            familyDto.AdminUsers = adminUsers;

            return Ok(familyDto);
        }
    }
}
