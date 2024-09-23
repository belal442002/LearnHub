using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomGenderAttribute : ValidationAttribute
    {
        public CustomGenderAttribute()
        {
            var defaultMessage = "Gender must be Male or Female";
            ErrorMessage ??= defaultMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string gender)
            {
                if (!gender.Equals("Male", StringComparison.OrdinalIgnoreCase)
                    && !gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
