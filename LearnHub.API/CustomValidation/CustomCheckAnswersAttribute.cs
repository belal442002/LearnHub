using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.QBAnswersDto;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.CustomValidation
{
    public class CustomCheckAnswersAttribute : ValidationAttribute
    {
        public CustomCheckAnswersAttribute()
        {
            var defaultMessage = "Wrong adding Answers format";
            ErrorMessage ??= defaultMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var answers = value as List<AddQBAnswerRequestDto>;
            if (answers == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            // Retrieve the QuestionTypeId from the validation context
            var questionTypeIdProperty = validationContext.ObjectType.GetProperty("QuestionTypeId");
            if (questionTypeIdProperty == null)
            {
                return new ValidationResult("QuestionTypeId property not found.");
            }

            var questionTypeId = (int)questionTypeIdProperty.GetValue(validationContext.ObjectInstance);

            if (!answers.Any(a => a.Answer_TF))
            {
                return new ValidationResult("The question must have at least one true answer");
            }

            switch ((Helper.QuestionType)questionTypeId)
            {
                case Helper.QuestionType.MultipleChoice:
                    if (answers.Count < 4)
                    {
                        return new ValidationResult("Multiple choice question must have at least four answers");
                    }
                    break;
                case Helper.QuestionType.TrueOrFalse:
                    if (answers.Count < 2)
                    {
                        return new ValidationResult("True or False question must have at least two answers");
                    }
                    break;
                case Helper.QuestionType.Essay:
                    if (answers.Count < 1)
                    {
                        return new ValidationResult("Essay question must have at least one answer");
                    }
                    break;
            }

            return ValidationResult.Success;
        }
    }
}