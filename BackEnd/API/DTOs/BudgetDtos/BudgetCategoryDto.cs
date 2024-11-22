namespace BackEnd.DTOs.BudgetDtos
{
    public class BudgetCategoryDto
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public List<TransactionDto> Transactions { get; set; } = [];
    }
}
