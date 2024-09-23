using System;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomAssignmentTypeAttribute : ValidationAttribute
    {
        public CustomAssignmentTypeAttribute()
        {
            var defaultMessage = "Assignment Type should be Quiz or Assignment";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string assignmentType)
            {
                if (!assignmentType.Equals("Quiz", StringComparison.OrdinalIgnoreCase)
                    && !assignmentType.Equals("Assignment", StringComparison.OrdinalIgnoreCase))
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
