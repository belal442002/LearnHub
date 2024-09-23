using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.QuestionBankDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class QuestionBankController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionBankController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           
        }

        [HttpGet]
        [Route("{questionId:int}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetById([FromRoute] int questionId)
        {
            if(await _unitOfWork.QuestionBank.GetByIdAsync(questionId) == null)
            {
                return NotFound(new { Message = "Question not found" });
            }
            return Ok(_mapper.Map<QuestionBankDto>(await _unitOfWork.QuestionBank.GetByIdWithIncludeAsync(questionId)));
        }

        [HttpGet]
        [Route("ByCourseId/{courseId}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetByCourseId([FromRoute] string courseId)
        {
            if(await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Course not found" });
            }    
            return Ok(_mapper.Map<List<QuestionBankDto>>(await _unitOfWork.QuestionBank.GetByCourseIdAsync(courseId)));
        }

        [HttpGet]
        [Route("ByInstructorId/{instructorId:int}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetByInstructorId([FromRoute] int instructorId)
        {
            if(await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Instructor not found"});
                }
            return Ok(_mapper.Map<List<QuestionBankDto>>(await _unitOfWork.QuestionBank.GetByInstructorIdAsync(instructorId)));
        }

        [HttpGet]
        [Route("ByCourseAndInstructorId/{courseId}/{instructorId:int}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetByCourseAndInstructorId([FromRoute] String courseId, [FromRoute] int instructorId)
        {
            if(await _unitOfWork.Course.GetByIdAsync(courseId) == null || await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Instructor or Course not found" });
            }
            return Ok(_mapper.Map<List<QuestionBankDto>>(await _unitOfWork.QuestionBank.GetByCourseAndInstructorIdAsync(courseId, instructorId)));
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _unitOfWork.QuestionBank.GetAllQuestionsWithIncludeAsync();
            return Ok(_mapper.Map<List<QuestionBankDto>>(questions));
        }

        [HttpGet]
        [Route("QuestionCountByTopic/{courseId}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetQuestionCountByCourseGroupedByTopic([FromRoute] string courseId)
        {
            if (await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Course not found" });
            }

            var questionCounts = await _unitOfWork.QuestionBank.GetQuestionCountByCoursePerTopicAsync(courseId);
            return Ok(questionCounts);
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddRange([FromBody] List<AddQuestionBankRequestDto> questions)
        {
            if (await _unitOfWork.Course.GetByIdAsync<string>(questions[0].CourseId) == null
                || await _unitOfWork.Instructor.GetByIdAsync<int>(questions[0].InstructorId) == null)
            {
                return NotFound(new { Message = "Instructor or Course not found" });
            }

            foreach (var question in questions) 
            {
                var quesitonDomainModel = _mapper.Map<QuestionBank>(question);
                await _unitOfWork.QuestionBank.AddAsync(quesitonDomainModel);  
            }
            return Ok(new { Message = "Questions added successfully" });
        }

        [HttpPut]
        [Route("UpdateQuestion/{QuestionId:int}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Update([FromRoute] int QuestionId, UpdateQuestionBankRequestDto updatedQuestion)
        {
            if (await _unitOfWork.QuestionBank.GetByIdAsync(QuestionId) == null)
            {
                return NotFound(new { Message = "Question not found" });
            }
            var questionDomainModel = _mapper.Map<QuestionBank>(updatedQuestion);

            if (!await _unitOfWork.QuestionBank.UpdateQuestionAsync(QuestionId, questionDomainModel))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating the question");
                return StatusCode(500, ModelState);
            }

            return Ok(new { Message = "Updated successfully" });
        }

        [HttpDelete]
        [Route("DeleteQuestion/{id:int}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var questionDomainModel = await _unitOfWork.QuestionBank
                    .GetByIdWithIncludeAsync(id);

                if (questionDomainModel == null)
                {
                    return NotFound(new { Message = "Question not found" });
                }

                // Begin transaction 
                await _unitOfWork.BeginTransactionAsync();

                // Delete answers first transaction
                foreach(var answer in questionDomainModel.Answers)
                {
                    if(!await _unitOfWork.QBAnswers.DeleteAnswerAsync(answer.AnswerId))
                    {
                        await _unitOfWork.RollbackAsync();
                        return BadRequest(new { Message = "Something went wrong while deleting" });
                    }
                }

                // Delete question second transaction
                if(!await _unitOfWork.QuestionBank.RemoveQuestionAsync(id))
                {
                    await _unitOfWork.RollbackAsync();
                    return BadRequest(new { Message = "Something went wrong while deleting" });
                }

                // Commit and save
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return BadRequest(new { Message = ex.Message });
            }

            return Ok(new { Message = "Deleted successfully" });
        }
    }
}
