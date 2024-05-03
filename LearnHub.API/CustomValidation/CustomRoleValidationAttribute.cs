using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomRoleValidationAttribute : ValidationAttribute
    {
        public CustomRoleValidationAttribute()
        {
            var defaultMessage = "Role must be Instructor or Student";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var role = value as String;
            if (role != null)
            {
                var student = Helper.Roles.Student.ToString();
                var instructor = Helper.Roles.Instructor.ToString();
                if (!role.Equals(student, StringComparison.OrdinalIgnoreCase) && !role.Equals(instructor, StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult("Role must be Instructor or Student");
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
