namespace BackEnd.DTOs.BudgetDtos.Requests
{
    public class UpdateMonthlyBillRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required decimal MonthlyAmount { get; set; }
    }
}
