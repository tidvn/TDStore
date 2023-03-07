using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TDStore.Models
{
    public class Payments
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string CustomerId { get; set; } = null!;
        public string Status { get; set; }
        public string gateway { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string token { get; set; }
        public string note { get; set; }
    }
}
