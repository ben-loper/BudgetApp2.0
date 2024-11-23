using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Models
{
    public class ApplicationUser : MongoUser
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal BiWeeklySalary { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PreviousPayDate { get; set; }

    }

    public class ApplicationRole : MongoRole
    {
    }
}
