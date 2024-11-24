namespace BackEnd.DTOs.BudgetDtos
{
    public class TransactionDto
    {
        public string? Id { get; set; }
        public required string BudgetCategoryId { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public required string Name { get; set; }
    }
}
