using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomRoleValidationAttribute : ValidationAttribute
    {
        public CustomRoleValidationAttribute()
        {
            var defaultMessage = "Roles must be Instructor, Student or Parent";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var role = value as String;
            if (role != null)
            {
                var student = Helper.Roles.Student.ToString();
                var instructor = Helper.Roles.Instructor.ToString();
                var parent = Helper.Roles.Parent.ToString();
                
                if (!role.Equals(student, StringComparison.OrdinalIgnoreCase) && 
                    !role.Equals(instructor, StringComparison.OrdinalIgnoreCase) &&
                    !role.Equals(parent, StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult("Roles must be Instructor, Student or Parent");
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
