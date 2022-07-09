using System.ComponentModel.DataAnnotations;

namespace eStoreClient.Utilities
{
    public class CapitalizedEachWordAttribute : ValidationAttribute
    {
        public CapitalizedEachWordAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var str = (string)value;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    str = str.Trim();
                    string[] words = str.Split(" ");
                    bool isAllUpper = true;
                    foreach (var word in words)
                    {
                        char[] chrs = word.ToCharArray();
                        if (!char.IsUpper(chrs[0]))
                        {
                            isAllUpper = false;
                        }
                        foreach (var chr in chrs)
                        {
                            if (char.IsDigit(chr) || !char.IsLetterOrDigit(chr))
                            {
                                return new ValidationResult("Digits and non-alphanumeric characters are not allowed.");
                            }
                        }
                    }
                    if (isAllUpper)
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            catch
            {
            }
            return new ValidationResult("Each leading letter must be capitalized. There may only be one whitespace between any two words.");
        }
    }
}
