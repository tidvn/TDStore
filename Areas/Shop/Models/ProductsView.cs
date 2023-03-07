using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TDStore.Models;

namespace TDStore.Areas.Shop.Models
{
    public class ProductsView
    {
        public Product Data { get; set; }

        public List<ImageData> Image { get; set; }
    }
}
