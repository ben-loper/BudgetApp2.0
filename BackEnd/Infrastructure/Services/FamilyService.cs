using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IFamilyRepository _familyRepository;

        public FamilyService(IFamilyRepository familyRepository)
        {
            _familyRepository = familyRepository;
        }

        public async Task<Family> CreateFamilyAsync(Family family, string userId)
        {
            var existingFamily = await _familyRepository.GetFamilyForUserIdAsync(userId);

            if (existingFamily != null) throw new UserAlreadyInFamilyException();

            family.AdminUserIds.Add(userId);

            return await _familyRepository.CreateFamilyAsync(family);
        }

        public async Task<Family> GetFamilyAsync(string userId)
        {
            return await _familyRepository.GetFamilyForUserIdAsync(userId);
        }

        public async Task<Family> AddUserToFamilyAsync(string familyId, string userId, FamilyRole role)
        {
            var family = await _familyRepository.GetFamilyByIdAsync(familyId)
                ?? throw new FamilyDoesNotExistException();

            var userIsInAFamily = await _familyRepository.GetFamilyForUserIdAsync(userId) != null;

            if (userIsInAFamily) throw new UserAlreadyInFamilyException();

            switch (role)
            {
                case FamilyRole.Admin:
                    family.AdminUserIds.Add(userId);
                    break;
                case FamilyRole.Member:
                    family.MemberUserIds.Add(userId);
                    break;
                default:
                    throw new RoleDoesNotExistException();
            }

            return await _familyRepository.UpdateFamilyAsync(family);
        }

        public async Task<Family> RemoveUserFromFamilyAsync(string familyId, string userId)
        {
            var family = await _familyRepository.GetFamilyByIdAsync(familyId)
                ?? throw new FamilyDoesNotExistException();

            var userIsInAdminList = family.AdminUserIds.Contains(userId);
            var userIsInMemberList = family.MemberUserIds.Contains(userId);

            if (!userIsInAdminList && !userIsInMemberList)
            {
                throw new UserDoesNotExistInFamily();
            }

            if (userIsInAdminList)
            {
                family.AdminUserIds.Remove(userId);
            }
            else
            {
                family.MemberUserIds.Remove(userId);
            }

            return await _familyRepository.UpdateFamilyAsync(family);
        }
    }
}
