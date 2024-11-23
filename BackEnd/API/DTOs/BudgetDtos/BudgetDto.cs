using Domain.Models;

namespace BackEnd.DTOs.BudgetDtos
{
    public class BudgetDto
    {
        public required string Id { get; set; }
        public decimal PayThisMonth { get; set; }
        public required List<MonthlyBillDto> MonthlyBills { get; set; } = [];
        public required List<BudgetCategory> BudgetCategories { get; set; } = [];
    }
}
