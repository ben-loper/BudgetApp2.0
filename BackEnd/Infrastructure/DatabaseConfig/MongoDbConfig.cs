using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DatabaseConfig
{
    public class MongoDbConfig
    {
        public required string DatabaseName { get; init; }
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required string IdentityCollectionName { get; set; }
        public string ConnectionString => $"mongodb://{Host}:{Port}/{DatabaseName}";
    }
}
