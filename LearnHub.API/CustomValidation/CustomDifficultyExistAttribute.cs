using LearnHub.API.Data;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomDifficultyExistAttribute : ValidationAttribute
    {
        public CustomDifficultyExistAttribute()
        {
            var defaultMessage = "Wrong Difficulty";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int difficultyId)
            {
                var _dbContext = validationContext.GetService(typeof(LearnHubDbContext)) as LearnHubDbContext;
                if (_dbContext != null)
                {
                    var difficulty = _dbContext.Difficulties.FirstOrDefault(d => d.Id == difficultyId);
                    if (difficulty == null)
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
