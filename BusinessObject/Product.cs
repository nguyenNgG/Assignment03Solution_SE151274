using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    [Table("Product")]
    public class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Column(TypeName = "nvarchar(256)")]
        public string? ProductName { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
