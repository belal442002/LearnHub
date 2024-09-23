using AutoMapper;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AssignmentQuestionDto;
using LearnHub.API.Models.Dto.EvaluationDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AssignmentQuestionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        public AssignmentQuestionController(IUnitOfWork unitOfWork, IMapper mapper, NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;

        }

        [HttpGet]
        [Route("GenerateAssignment/{studentId:int}/{courseId}")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GenerateAssignment([FromRoute] int studentId, [FromRoute] String courseId)
        {
            if(await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect courseId" });
            }
            if(await _unitOfWork.Student.GetByIdAsync(studentId) == null)
            {
                return NotFound(new { Message = "Incorrect studentId" });
            }

            try
            {
                var questions = await _unitOfWork.AssignmentQuestion
                .GenerateAssignmentQuestionsWithDurationAsync(studentId, courseId);
                return Ok(questions);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            
        }

        [HttpPost]
        [Route("UploadAssignment")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitStudentAnswers([FromBody] StudentAnswersSubmissionDto submission)
        {
            if (await _unitOfWork.Assignment.GetByIdAsync(submission.AssignmentId) == null)
            {
                return NotFound(new { Message = "Incorrect assignmentId" });
            }
            if (await _unitOfWork.Student.GetByIdAsync(submission.StudentId) == null)
            {
                return NotFound(new { Message = "Incorrect studentId" });
            }
            if(await _unitOfWork.Evaluation.GetEvaluationAsync(submission.AssignmentId, submission.StudentId) != null)
            {
                return BadRequest(new { Message = $"Student with id {submission.StudentId} has already submit this quiz"});
            }

            try
            {
                var evaluation = await _unitOfWork.AssignmentQuestion.SaveStudentAnswersAsync(submission);

                var student = await _unitOfWork.Student
                    .GetStudentByIdWithAccountAsync(submission.StudentId);

                //var studentParent = student!.Parent.AccountId;
                //await _notificationService.SendGradeNotification(studentParent
                //    , $"Your student {evaluation.StudentId} grades in course {submission.CourseId} updated");

                return Ok(_mapper.Map<EvaluationDto>(evaluation));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{courseId}")]
        //[Authorize(Roles = "Instructor, Student")]
        public async Task<IActionResult> GetAll([FromRoute]String courseId, 
            [FromQuery]int? studentId, [FromQuery]int? assignmentId,
            [FromQuery]int? assignmentNumber)
        {
            if (await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect course Id" });
            }
            var assignmentQuestionsDomainModel = await _unitOfWork.AssignmentQuestion
                .GetAllAsync(courseId, studentId, assignmentId, assignmentNumber);

            return Ok(_mapper.Map<List<AssignmentQuestionDto>>(assignmentQuestionsDomainModel));
        }
    }
}
