using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TDStore.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        public decimal ListedPrice { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }
        public string Description { get; set; }

        public List<string> Category { get; set; }

        public List<object> Details { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public List<String?> Images { get; set; }

    }
}

