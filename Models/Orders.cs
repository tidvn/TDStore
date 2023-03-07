using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TDStore.Models
{
    public class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string CustomerId { get; set; } = null!;

        public string PaymentId { get; set; }

        public string PaymentStatus { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public List<ItemOrder> Items { get; set; }

        public object Shipping { get; set; }


    }
    public class ItemOrder
    {
        public Product Product { get; set; }

        public decimal Quantity { get; set; }

        public decimal Discount { get; set; }

    }

}

