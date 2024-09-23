using LearnHub.API.Data;
using LearnHub.API.Helper;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AssignmentQuestionDto;
using LearnHub.API.Models.Dto.QuestionBankDto;
using Microsoft.EntityFrameworkCore;



namespace LearnHub.API.Repositories
{
    public class AssignmentQuestionRepository : Repository<AssignmentQuestion>, IAssignmentQuestionRepository
    {
        private readonly LearnHubDbContext _dbContext;

        public AssignmentQuestionRepository(LearnHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Old one
        public async Task<ICollection<AssignmentQuestionsGeneratedDto>> GenerateAssignmentQuestionsAsync(int studentId, String courseId)
        {
            // Get assignment
            var assignmentDomainModel = await _dbContext.Assignments
                .Where(a => a.CourseId.Equals(courseId))
                .OrderByDescending(a => a.AssignmentNumber).FirstOrDefaultAsync();

            if(assignmentDomainModel == null)
            {
                throw new Exception("No assignments created");
            }

            // Configuration of assignment
            var assignmentConfigTopics = await _dbContext.AssignmentConfigTopics
                .Where(a => a.AssignmentConfigId == assignmentDomainModel.AssignmentConfigId)
                .ToListAsync();

            // Get questions of course
            var questions = await _dbContext.Questions
                .Where(q => q.CourseId.Equals(courseId))
                .Include(q => q.Topic)
                .Include(q => q.Difficulty)
                .Include(q => q.Answers)
                .ToListAsync();

            // Get each topic with number of question
            var questionsCount = questions
                .GroupBy(q => new { q.TopicId, q.DifficultyId, q.Topic.TopicName, q.Difficulty.Name})
                .Select(g => new QuestionCountByTopicDto
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    DifficultyId = g.Key.DifficultyId,
                    DifficultyLevel = g.Key.Name,
                    QuestionCount = g.Count()
                });

            // ensure that numer of questions in assignment less than that in QuestionBank
            foreach(var topic in assignmentConfigTopics)
            {
                foreach(var question in questionsCount)
                {
                    if(topic.TopicId == question.TopicId 
                        && topic.DifficultyId == question.DifficultyId 
                        && topic.NumberOfQuestions > question.QuestionCount)
                    {
                        throw new Exception("AssignmentConfigTopics has number of questions greater than that in QuestionBank for this course");
                    }
                }
            }

            // to store questions in
            var assignmentQuestions = new List<AssignmentQuestionsGeneratedDto>(); 

            // Generate questions per each topic & difficulty
            foreach(var topic in assignmentConfigTopics)
            {
                var allowedQuestions = questions
                    .Where(q => q.TopicId == topic.TopicId && q.DifficultyId == topic.DifficultyId)
                    .OrderBy(q => Guid.NewGuid()) // Randomize the order
                    .Take(topic.NumberOfQuestions); 

                foreach(var question in allowedQuestions)
                {
                    var assignmentQuestion = new AssignmentQuestionsGeneratedDto
                    {
                        QuestionId = question.QuestionId,
                        QuestionText = question.QuestionText,
                        AssignmentId = assignmentDomainModel.Id,
                        StudentId = studentId,
                        StudentAnswer = String.Empty
                    };

                    // if the question is MultipleChoice
                    if (((LearnHub.API.Helper.QuestionType)question.QuestionTypeId).ToString()
                        == LearnHub.API.Helper.QuestionType.MultipleChoice.ToString())
                    {
                        var answers = question.Answers.OrderBy(a => Guid.NewGuid()).ToList();
                        if (answers.Count < 4)
                            throw new Exception("Number of answers for MultipleChoice questions shouldn't be less than 4");

                        assignmentQuestion.ChoiceA = answers.ElementAtOrDefault(0)?.AnswerText;
                        assignmentQuestion.ChoiceB = answers.ElementAtOrDefault(1)?.AnswerText;
                        assignmentQuestion.ChoiceC = answers.ElementAtOrDefault(2)?.AnswerText;
                        assignmentQuestion.ChoiceD = answers.ElementAtOrDefault(3)?.AnswerText;
                    }

                    assignmentQuestions.Add(assignmentQuestion);
                }
            }

            return assignmentQuestions;
        }

        // new One with duration
        public async Task<AssignmentQuestionsGeneratedWithDurationDto> GenerateAssignmentQuestionsWithDurationAsync(int studentId, String courseId)
        {
            // Get assignment
            var assignmentDomainModel = await _dbContext.Assignments
                .Where(a => a.CourseId.Equals(courseId))
                .Include(a => a.AssignmentConfig)
                .OrderByDescending(a => a.AssignmentNumber).FirstOrDefaultAsync();

            if (assignmentDomainModel == null)
            {
                throw new Exception("No assignments created");
            }

            // Configuration of assignment
            var assignmentConfigTopics = await _dbContext.AssignmentConfigTopics
                .Where(a => a.AssignmentConfigId == assignmentDomainModel.AssignmentConfigId)
                .ToListAsync();

            // Get questions of course
            var questions = await _dbContext.Questions
                .Where(q => q.CourseId.Equals(courseId))
                .Include(q => q.Topic)
                .Include(q => q.Difficulty)
                .Include(q => q.Answers)
                .ToListAsync();

            // Get each topic with number of question
            var questionsCount = questions
                .GroupBy(q => new { q.TopicId, q.DifficultyId, q.Topic.TopicName, q.Difficulty.Name })
                .Select(g => new QuestionCountByTopicDto
                {
                    TopicId = g.Key.TopicId,
                    TopicName = g.Key.TopicName,
                    DifficultyId = g.Key.DifficultyId,
                    DifficultyLevel = g.Key.Name,
                    QuestionCount = g.Count()
                });

            // ensure that numer of questions in assignment less than that in QuestionBank
            foreach (var topic in assignmentConfigTopics)
            {
                foreach (var question in questionsCount)
                {
                    if (topic.TopicId == question.TopicId
                        && topic.DifficultyId == question.DifficultyId
                        && topic.NumberOfQuestions > question.QuestionCount)
                    {
                        throw new Exception("AssignmentConfigTopics has number of questions greater than that in QuestionBank for this course");
                    }
                }
            }

            // to store questions
            var assignmentQuestions = new AssignmentQuestionsGeneratedWithDurationDto
            {
                AssignmentId = assignmentDomainModel.Id,
                StudentId = studentId,
                Duration = SystemService
                        .GetDurationInMinutes(assignmentDomainModel.AssignmentConfig.EndDate,
                                              assignmentDomainModel.AssignmentConfig.StartDate),
                QuestionsGenerated = new List<QuestionsGeneratedDto>()
            };

            // Generate questions per each topic & difficulty
            foreach (var topic in assignmentConfigTopics)
            {
                var allowedQuestions = questions
                    .Where(q => q.TopicId == topic.TopicId && q.DifficultyId == topic.DifficultyId)
                    .OrderBy(q => Guid.NewGuid()) // Randomize the order
                    .Take(topic.NumberOfQuestions);

                foreach (var question in allowedQuestions)
                {
                    var generatedQusetion = new QuestionsGeneratedDto
                    {
                        QuestionId = question.QuestionId,
                        QuestionText = question.QuestionText,
                        StudentAnswer = String.Empty
                    };
                    // if the question is MultipleChoice
                    if (((LearnHub.API.Helper.QuestionType)question.QuestionTypeId).ToString()
                        == LearnHub.API.Helper.QuestionType.MultipleChoice.ToString())
                    {
                        var answers = question.Answers.OrderBy(a => Guid.NewGuid()).ToList();
                        if (answers.Count < 4)
                            throw new Exception("Number of answers for MultipleChoice questions shouldn't be less than 4");
                        
                         generatedQusetion.ChoiceA = answers.ElementAtOrDefault(0)?.AnswerText;
                         generatedQusetion .ChoiceB = answers.ElementAtOrDefault(1)?.AnswerText;
                         generatedQusetion.ChoiceC = answers.ElementAtOrDefault(2)?.AnswerText;
                         generatedQusetion.ChoiceD = answers.ElementAtOrDefault(3)?.AnswerText;
                    }

                    assignmentQuestions.QuestionsGenerated.Add(generatedQusetion);
                }
            }

            return assignmentQuestions;
        }

        public async Task<Evaluation> SaveStudentAnswersAsync(StudentAnswersSubmissionDto submission)
        {
            var evaluation = new Evaluation
            {
                StudentId = submission.StudentId,
                AssignmentId = submission.AssignmentId,
                Grade = 0, 
            };

            var correctAnswersCount = 0;
            var numberOfQuestions = submission.Answers.Count;

            var assignmentQuestions = new List<AssignmentQuestion>();

            foreach (var answer in submission.Answers)
            {
                var correctAnswer = await _dbContext.QBAnswers
                    .Where(qb => qb.QuestionId == answer.QuestionId && qb.Answer_TF == true)
                    .Include(qb => qb.Question)
                    .FirstOrDefaultAsync();

                if (correctAnswer == null)
                {
                    throw new Exception("The question has no answer");
                }

                try
                {
                    bool isCorrect = await CheckAnswer(correctAnswer.Question.QuestionTypeId, 
                        correctAnswer.AnswerText, 
                        answer.StudentAnswer??string.Empty);

                    if (isCorrect)
                    {
                        correctAnswersCount++;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                var assignmentQuestion = new AssignmentQuestion
                {
                    StudentId = submission.StudentId,
                    AssignmentId = submission.AssignmentId,
                    QuestionId = answer.QuestionId,
                    ChoiceA = answer.ChoiceA,
                    ChoiceB = answer.ChoiceB,
                    ChoiceC = answer.ChoiceC,
                    ChoiceD = answer.ChoiceD,
                    CorrectAnswer = correctAnswer.AnswerText,
                    StudentAnswer = answer.StudentAnswer
                };

                assignmentQuestions.Add(assignmentQuestion);

            }

            // Calculate the grade
            var maxScore = await _dbContext.Assignments
                .Where(assignment => assignment.CourseId.Equals(submission.CourseId))
                .Include(a => a.AssignmentConfig)
                .OrderByDescending(a => a.AssignmentNumber)
                .Select(a => a.AssignmentConfig.MaxScore)
                .FirstOrDefaultAsync();

            evaluation.Grade = CalculateGrade(correctAnswersCount, numberOfQuestions, maxScore);
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Evaluations.AddAsync(evaluation);
                    await _dbContext.AssignmentQuestions.AddRangeAsync(assignmentQuestions);
                    await transaction.CommitAsync();
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    await transaction.DisposeAsync();
                    throw new Exception(e.Message);
                }
            }
            var assignmentDomainModel = await _dbContext.Assignments
                .Where(a => a.Id == evaluation.AssignmentId)
                .Include(a => a.AssignmentConfig).FirstOrDefaultAsync();
            if(assignmentDomainModel != null)
            {
                evaluation.Assignment =  assignmentDomainModel;
            }
            
            return evaluation;
        }

        private async Task<bool> CheckAnswer(int questionTypeId, string correctAnswer, string studentAnswer)
        {
            
            if (questionTypeId == (int)Helper.QuestionType.MultipleChoice ||
                questionTypeId == (int)Helper.QuestionType.TrueOrFalse)
            {
                return string.Equals(correctAnswer, studentAnswer, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Handle essay question type here
                try
                {
                    var similarityCheckerAPI = new SimilarityCheckerAPI();
                    return await similarityCheckerAPI.CheckSimilarity(studentAnswer, correctAnswer);

                } 
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private double CalculateGrade(int correctAnswers, int totalQuestions, int maxScore)
        {
            // grade calculation logic here


            // For simplicity, let's assume each question is worth 1 point and grade is percentage of correct answers
            var percentage = (double)correctAnswers / totalQuestions;

            return percentage * maxScore;
        }

        public async Task<ICollection<AssignmentQuestion>> GetAllAsync(string courseId, int? studentId, int? assignmentId, int? assignmentNumber)
        {
            var assignmentQuestions = _dbContext.AssignmentQuestions
                .Include(aq => aq.Assignment)
                .Include(aq => aq.Question)
                .Where(aq => aq.Assignment.CourseId.Equals(courseId))
                .AsQueryable();
                
            if(studentId != null)
            {
                assignmentQuestions = assignmentQuestions.Where(a => a.StudentId == studentId);
            }
            if (assignmentId != null)
            {
                assignmentQuestions = assignmentQuestions.Where(a => a.AssignmentId == assignmentId);
            }
            if (assignmentNumber != null)
            {
                assignmentQuestions = assignmentQuestions.Where(a => a.Assignment.AssignmentNumber == assignmentNumber);
            }

            return await assignmentQuestions.ToListAsync();
        }
    }
}
