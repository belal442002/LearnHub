using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomDigitsOnlyAttribute : ValidationAttribute
    {
        public CustomDigitsOnlyAttribute()
        {
            var defaultMessage = "NationalId should contains digits only and the length should be 14";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is String nationalId)
            {
                if (nationalId.Length != 14)
                    return new ValidationResult("NationalId should be 14 digits");

                if (!long.TryParse(nationalId, out long result))
                    return new ValidationResult("NationalId should be digits only");
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
