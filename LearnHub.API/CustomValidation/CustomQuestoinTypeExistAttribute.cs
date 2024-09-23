using LearnHub.API.Data;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomQuestionTypeExistAttribute : ValidationAttribute
    {
        public CustomQuestionTypeExistAttribute()
        {
            var defaultMessage = "Wrong Question Type";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int questionTypeId)
            {
                var _dbContext = validationContext.GetService(typeof(LearnHubDbContext)) as LearnHubDbContext;
                if (_dbContext != null)
                {
                    var questionType = _dbContext.QuestionTypes.FirstOrDefault(q => q.Id == questionTypeId);
                    if (questionType == null)
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
