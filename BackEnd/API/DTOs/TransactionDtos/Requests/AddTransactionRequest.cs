namespace BackEnd.DTOs.TransactionDtos.Requests
{
    public class AddTransactionRequest
    {
        public required string BudgetCategoryId { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required decimal Amount { get; set; }
        public required string Name { get; set; }
    }
}
