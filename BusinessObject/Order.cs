using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    [Table("Order")]
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        public string? MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        [Required]
        public DateTime? RequiredDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? ShippedDate { get; set; }

        [Column(TypeName = "money")]
        [Required]
        [Range(0, 999999)]
        public decimal? Freight { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
