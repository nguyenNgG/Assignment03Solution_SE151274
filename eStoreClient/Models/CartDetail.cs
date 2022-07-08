using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Models
{
    public class CartDetail
    {
        public ProductItem ProductItem { get; set; }
        public short Quantity { get; set; } = 0;

        [Range(0, 100)]
        public decimal Discount { get; set; } = 0;
    }
}
