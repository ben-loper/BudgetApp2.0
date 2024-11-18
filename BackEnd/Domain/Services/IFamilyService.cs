using Domain.Enums;
using Domain.Models;
using System.Security.Claims;

namespace Domain.Services
{
    public interface IFamilyService
    {
        Task<Family> CreateFamilyAsync(Family family, string userId);
        Task<Family> GetFamilyAsync(string userId);
        Task<Family> GetFamilyByIdAsync(string familyId);
        Task<Family> AddUserToFamilyAsync(string familyId, string userId, FamilyRole role);
        Task<Family> RemoveUserFromFamilyAsync(string familyId, string userId);
    }
}
