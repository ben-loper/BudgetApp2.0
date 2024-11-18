using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models
{
    public class BudgetCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }

        public required List<Transaction> Transactions { get; set; } = [];
    }
}
