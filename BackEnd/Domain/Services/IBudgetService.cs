using Domain.Models;

namespace Domain.Services
{
    public interface IBudgetService
    {
        Task<Family> CreateBudgetForFamilyAsync(string familyId, Budget budget);
        Task<Family> CreateCategoryForBudgetAsync(string familyId, BudgetCategory category);
        Task<Family> UpdateCategoryAsync(string familyId, string budgetCategoryId, string budgetName, decimal amount);
        Task<Family> DeleteCategoryAsync(string familyId, string categoryId);
        Task<Family> CreateMonthlyBillAsync(string familyId, MonthlyBill monthlyBill);
        Task<Family> UpdateMonthlyBillAsync(string familyId, string monthlyBillId, string name, decimal monthlyAmount);
        Task<Family> DeleteMonthlyBillAsync(string familyId, string monthlyBillId);
    }
}
