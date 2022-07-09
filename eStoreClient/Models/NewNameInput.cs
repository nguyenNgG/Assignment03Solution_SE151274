using eStoreClient.Utilities;
using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Models
{
    public class NewNameInput
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [CapitalizedEachWord]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [CapitalizedEachWord]
        public string LastName { get; set; }
    }
}
