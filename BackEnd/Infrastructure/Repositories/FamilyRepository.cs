using AspNetCore.Identity.Mongo.Mongo;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Models;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly IMongoCollection<Family> _familyCollection;

        public FamilyRepository(MongoDbContext context)
        {
            _familyCollection = context.Families;
        }

        public async Task<Family> CreateFamilyAsync(Family family)
        {
            await _familyCollection.InsertOneAsync(family);
            return family;
        }

        public async Task<Family> GetFamilyForUserIdAsync(string userId)
        {
            return await _familyCollection.FirstOrDefaultAsync(f => f.AdminUserIds.Contains(userId)
                                                                || f.MemberUserIds.Contains(userId));
        }

        public async Task<Family> UpdateFamilyAsync(Family family)
        {
            var filter = Builders<Family>.Filter.Eq(f => f.Id, family.Id);

            var result = await _familyCollection.ReplaceOneAsync(filter, family);

            return family;
        }

        public async Task<Family> GetFamilyByIdAsync(string familyId)
        {
            return await _familyCollection.FirstOrDefaultAsync(f => f.Id == familyId);
        }
    }
}
