using AspNetCore.Identity.Mongo.Mongo;
using BackEnd.DTOs.FamilyDtos;
using BackEnd.Exceptions;
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
                Id = family.Id.ToString(),
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
                Id = family.Id,
                Name = family.Name,
                AdminUsers = [],
                Members = []
            };

            foreach (var userId in family.AdminUserIds)
            {
                var person = await _userManager.FindByIdAsync(userId);

                familyDto.AdminUsers.Add(person.UserName);
            }

            foreach (var userId in family.MemberUserIds)
            {
                var person = await _userManager.FindByIdAsync(userId);

                familyDto.Members.Add(person.UserName);
            }

            return Ok(familyDto);
        }

        [HttpPut]
        public async Task<ActionResult<FamilyDto>> AddUserToFamily(string familyId, string username, bool isAdmin = false)
        {
            string? userId = null;

            try
            {
                userId = await GetUserIdByUsername(username);
            }
            catch (UserDoesNotExistException)
            {
                return BadRequest("User does not exist");
            }

            var family = await _familyCollection.FirstOrDefaultAsync(family => family.Id == familyId);

            if (family == null) return NotFound("No family exists with the given Id");

            var existingFamily = await _familyCollection.FirstOrDefaultAsync(f => f.AdminUserIds.Contains(userId)
                                                                || f.MemberUserIds.Contains(userId)
                                                            );

            if (existingFamily != null) return BadRequest("User is already in a family");

            var update = Builders<Family>.Update.AddToSet("MemberUserIds", userId);

            await _familyCollection.UpdateOneAsync(f => f.Id == familyId, update);

            var updatedFamily = await _familyCollection.FirstOrDefaultAsync(f => f.Id == familyId);

            var familyDto = new FamilyDto()
            {
                Id = familyId,
                Name = updatedFamily.Name,
                AdminUsers = [],
                Members = []
            };

            foreach (var adminUserId in updatedFamily.MemberUserIds)
            {
                var person = await _userManager.FindByIdAsync(userId);

                familyDto.Members.Add(person.UserName);
            }

            return familyDto;
        }
    }
}
