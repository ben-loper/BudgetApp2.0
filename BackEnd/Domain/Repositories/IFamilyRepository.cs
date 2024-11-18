using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IFamilyRepository
    {
        Task<Family> GetFamilyForUserIdAsync(string userId);
        Task<Family> CreateFamilyAsync(Family family);
        Task<Family> AddUserToFamilyAsync(string username, FamilyRole role);
        Task<Family> RemoveUserFromFamilyAsync(string username);
        Task<Family> UpdateFamilyAsync(Family family);
        Task<Family> GetFamilyByIdAsync(string familyId);
    }
}
