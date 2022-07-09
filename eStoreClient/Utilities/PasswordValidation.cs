using eStoreClient.Models;
using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Utilities
{
    public class PasswordValidation : ValidationAttribute
    {
        public PasswordValidation() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var str = (string)value;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    var hasLowercase = false;
                    var hasUppercase = false;
                    var hasDigit = false;
                    var hasNonAlphanumeric = false;
                    var hasValidLength = false;

                    if (str.Length >= 6) hasValidLength = true;

                    char[] chars = str.ToCharArray();
                    foreach (var chr in chars)
                    {
                        if (char.IsLower(chr)) hasLowercase = true;
                        if (char.IsUpper(chr)) hasUppercase = true;
                        if (char.IsDigit(chr)) hasDigit = true;
                        if (!char.IsLetterOrDigit(chr)) hasNonAlphanumeric = true;
                    }

                    if (hasLowercase && hasUppercase && hasDigit && hasNonAlphanumeric && hasValidLength)
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            catch
            {
            }
            return new ValidationResult("Password must contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character and must be at least six characters long.");
        }
    }
}
