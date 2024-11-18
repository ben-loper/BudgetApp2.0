using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IFamilyRepository _familyRepo;

        public BudgetService(IFamilyRepository familyRepo)
        {
            _familyRepo = familyRepo;
        }

        public async Task<Family> CreateBudgetForFamilyAsync(string familyId, Budget budget)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget != null) throw new BudgetAlreadyExistsForFamilyException();

            family.Budget = budget;

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public Task<Family> CreateCategoryForBudgetAsync(string budgetId, BudgetCategory category)
        {
            throw new NotImplementedException();
        }

        public Task<Family> DeleteCategoryAsync(string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<Family> UpdateCategoryAsync(string categoryId, BudgetCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
