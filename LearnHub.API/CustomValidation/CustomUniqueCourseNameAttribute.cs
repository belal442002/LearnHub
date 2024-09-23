using LearnHub.API.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomUniqueCourseNameAttribute : ValidationAttribute
    {
        public CustomUniqueCourseNameAttribute()
        {
            var defaultMessage = "Course name must be text";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string courseName)
            {
                var _dbContext = validationContext.GetService(typeof(LearnHubDbContext)) as LearnHubDbContext;
                if (_dbContext != null)
                {
                    var course = _dbContext.Courses.FirstOrDefault(c => c.Name.Trim().ToLower().Equals(courseName.Trim().ToLower()));
                    if (course != null)
                    {
                        return new ValidationResult("Course Name must be unique");
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
