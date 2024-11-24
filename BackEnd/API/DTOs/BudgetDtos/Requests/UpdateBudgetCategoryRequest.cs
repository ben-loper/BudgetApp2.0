using Domain.Models;

namespace BackEnd.DTOs.BudgetDtos
{
    public class UpdateBudgetCategoryRequest
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
    }
}
