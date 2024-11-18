using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEnd.DTOs.BudgetDtos
{
    public class MonthlyBillDto
    {
        public required string Id { get; set; }
        public decimal MonthlyAmount { get; set; }
        public required string Name { get; set; }
    }
}
