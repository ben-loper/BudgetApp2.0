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

        public async Task<Family> DeleteCategoryAsync(string familyId, string categoryId)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var budgetCategory = family.Budget.BudgetCategories.Where(b => b.Id == categoryId).FirstOrDefault();

            if (budgetCategory == null) throw new BudgetCategoryDoesNotExistException();

            family.Budget.BudgetCategories.Remove(budgetCategory);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> DeleteMonthlyBillAsync(string familyId, string monthlyBillId)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var monthlyBill = family.Budget.MonthlyBills.Where(b => b.Id == monthlyBillId).FirstOrDefault();

            if (monthlyBill == null) throw new MonthlyBillDoesNotExistException();

            family.Budget.MonthlyBills.Remove(monthlyBill);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> UpdateCategoryAsync(string familyId, string budgetCategoryId, string budgetName, decimal amount)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var budgetCategory = family.Budget.BudgetCategories.Where(b => b.Id == budgetCategoryId).FirstOrDefault();

            if (budgetCategory == null) throw new BudgetCategoryDoesNotExistException();

            budgetCategory.Amount = amount;
            budgetCategory.Name = budgetName;

            family.Budget.BudgetCategories.Remove(budgetCategory);

            family.Budget.BudgetCategories.Add(budgetCategory);

            return await _familyRepo.UpdateFamilyAsync(family);
        }

        public async Task<Family> UpdateMonthlyBillAsync(string familyId, string monthlyBillId, string name, decimal monthlyAmount)
        {
            var family = await _familyRepo.GetFamilyByIdAsync(familyId);

            if (family == null || family.Id == null) throw new FamilyCouldNotBeFoundException();

            if (family.Budget == null) throw new BudgetDoesNotExistForFamilyException();

            var monthlyBill = family.Budget.MonthlyBills.Where(b => b.Id == monthlyBillId).FirstOrDefault();

            if (monthlyBill == null) throw new MonthlyBillDoesNotExistException();

            family.Budget.MonthlyBills.Remove(monthlyBill);

            monthlyBill.Name = name;
            monthlyBill.MonthlyAmount = monthlyAmount;

            family.Budget.MonthlyBills.Add(monthlyBill);

            return await _familyRepo.UpdateFamilyAsync(family);
        }
    }
}
