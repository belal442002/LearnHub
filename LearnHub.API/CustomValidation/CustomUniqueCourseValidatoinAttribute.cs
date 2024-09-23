using LearnHub.API.Data;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomUniqueCourseValidatoinAttribute : ValidationAttribute
    {
        
        public CustomUniqueCourseValidatoinAttribute()
        {
            var defaultMessage = "Course ID must be unique";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string courseId)
            {
                var _dbContext = validationContext.GetService(typeof(LearnHubDbContext)) as LearnHubDbContext;
                if (_dbContext != null)
                {
                    var course = _dbContext.Courses.Find(courseId);
                    if (course != null)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
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