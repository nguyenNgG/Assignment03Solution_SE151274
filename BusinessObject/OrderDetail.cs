using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Discount { get; set; }
    }
}
