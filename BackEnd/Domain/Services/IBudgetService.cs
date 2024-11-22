using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IBudgetService
    {
        Task<Family> CreateBudgetForFamilyAsync(string familyId, Budget budget);
        Task<Family> CreateCategoryForBudgetAsync(string familyId, BudgetCategory category);
        Task<Family> UpdateCategoryAsync(string categoryId, BudgetCategory category);
        Task<Family> DeleteCategoryAsync(string categoryId);
    }
}
