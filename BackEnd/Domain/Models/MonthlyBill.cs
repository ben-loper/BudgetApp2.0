﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models
{
    public class MonthlyBill
    {
        public MonthlyBill()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MonthlyAmount { get; set; }

        public required string Name { get; set; }
    }
}