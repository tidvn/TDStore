using System.ComponentModel.DataAnnotations;
using TDStore.Models;

namespace TDStore.Areas.Shop.Models
{
    public class CartItemView
    {
        [Key]
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
