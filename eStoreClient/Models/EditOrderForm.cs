using System;
using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Models
{
    public class EditOrderForm
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Shipped Date")]
        [Required]
        public DateTime? ShippedDate { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal? Freight { get; set; }
    }
}
