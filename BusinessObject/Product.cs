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

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Column(TypeName = "nvarchar(256)")]
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(1, 999999)]
        public decimal Weight { get; set; }

        [Column(TypeName = "money")]
        [Range(0, 999999)]
        public decimal UnitPrice { get; set; }

        [Range(0, 999999)]
        public int UnitsInStock { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
