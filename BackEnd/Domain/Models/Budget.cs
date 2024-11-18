using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models
{
    public class Budget
    {
        public Budget()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required List<MonthlyBill> MonthlyBills { get; set; } = [];

        public required List<BudgetCategory> BudgetCategories { get; set; } = [];
    }
}
