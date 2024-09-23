using LearnHub.API.Data;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomTopicExistAttribute : ValidationAttribute
    {
        public CustomTopicExistAttribute()
        {
            var defaultMessage = "Wrong Topic";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int topicId)
            {
                var _dbContext = validationContext.GetService(typeof(LearnHubDbContext)) as LearnHubDbContext;
                if (_dbContext != null)
                {
                    var topic = _dbContext.Topics.FirstOrDefault(t => t.Id == topicId);
                    if (topic == null)
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
