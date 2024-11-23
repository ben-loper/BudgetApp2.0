using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using MongoDB.Bson;

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

        public async Task<Family> CreateCategoryForBudgetAsync(string familyId, BudgetCategory category)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            family.Budget.BudgetCategories.Add(category);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> CreateMonthlyBillAsync(string familyId, MonthlyBill monthlyBill)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            family.Budget.MonthlyBills.Add(monthlyBill);

            return await _familyRepo.UpdateFamilyAsync(family);
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
