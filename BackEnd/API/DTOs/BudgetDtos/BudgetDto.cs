using Domain.Models;

namespace BackEnd.DTOs.BudgetDtos
{
    public class BudgetDto
    {
        public required string Id { get; set; }
        public decimal PayThisMonth { get; set; }
        public decimal ProjectRemaining { get; set; }
        public decimal ActualRemaining { get; set; }
        public required List<MonthlyBillDto> MonthlyBills { get; set; } = [];
        public required List<BudgetCategoryDto> BudgetCategories { get; set; } = [];
    }
}
