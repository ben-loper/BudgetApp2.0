using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models
{
    public class Family
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Name { get; set; }
        public List<string> AdminUserIds { get; set; } = []; 
        public List<string> MemberUserIds { get; set; } = [];
    }
}
