using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Models
{
    public class LoginInput
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
