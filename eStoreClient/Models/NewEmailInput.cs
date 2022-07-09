using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Models
{
    public class NewEmailInput
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
