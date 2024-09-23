using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AssignmentDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        public AssignmentController(IUnitOfWork unitOfWork, IMapper mapper, NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        [HttpGet]
        [ValidationModel]
        [Route("{courseId}")]
        //[Authorize(Roles = "Instructor, Student")]

        public async Task<IActionResult> GetAssignmentsByCourseId([FromRoute] String courseId)
        {
            if (await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            { 
                return NotFound(new { Message = "Course not found" });
            }

            var assignments = await _unitOfWork.Assignment.GetAllAssignmentsByCourseIdAsync(courseId);

            return Ok(_mapper.Map<List<AssignmentDto>>(assignments));
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]

        public async Task<IActionResult> AddAssignment([FromBody] AddAssignmentConfigRequestDto addAssignmentConfigRequestDto)
        {
            if (await _unitOfWork.Course.GetByIdAsync(addAssignmentConfigRequestDto.CourseId) == null)
            {
                return NotFound(new { Message = "Course Id not found" });
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // create assignment configuration first T
                var assignmentConfiguration = _mapper.Map<AssignmentConfig>(addAssignmentConfigRequestDto);

                var added = await _unitOfWork.AssignmentConfig.AddAsync(assignmentConfiguration);

                if (! added)
                {
                    await _unitOfWork.RollbackAsync();
                    return BadRequest(new { Message = "something went wrong while adding assignment configuration" });
                }

                // create assignment second T

                var addedAssignment = await _unitOfWork.Assignment
                    .AddAssignmentAsync(addAssignmentConfigRequestDto.CourseId, assignmentConfiguration.Id);

                if (! addedAssignment)
                {
                    await _unitOfWork.RollbackAsync();
                    return BadRequest(new { Message = "something went wrong while adding assignment" });
                }

                // commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return BadRequest(ex.Message);
            }

            // new
            //var studentCourses = await _unitOfWork.StudentCourse
            //    .GetStudentCoursesByCourseIdAsync(addAssignmentConfigRequestDto.CourseId, 
            //    SystemService.GetSemesterByMonth(), DateTime.UtcNow.Year);
            //foreach (var studentCourse in studentCourses)
            //{

            //    // Notify students using AccountId
            //    await _notificationService.SendQuizNotification(studentCourse.Student.AccountId
            //        , $"A new quiz has been uploaded in {addAssignmentConfigRequestDto.CourseId}.");
            //}

            return Ok(new { Message = "Added successfully" });
        }

        [HttpPut]
        [ValidationModel]
        [Route("{assignmentId:int}")]
        //[Authorize(Roles = "Instructor")]

        public async Task<IActionResult> UpdateAssignmentCongiguration([FromRoute] int assignmentId
            , [FromBody] UpdateAssignmentRequestDto updateAssignmentRequestDto)
        {
            if (await _unitOfWork.Assignment.GetByIdAsync(assignmentId) == null)
            {
                return NotFound(new { Message = "Assignment Id not found" });
            }
            var assginmentDomainModel = _mapper.Map<Assignment>(updateAssignmentRequestDto);

            var updated = await _unitOfWork.Assignment.UpdateAssignmentAsync(assignmentId, assginmentDomainModel);

            if (!updated)
            {
                return BadRequest(new { Message = "Something went wrong while updataing" });
            }

            return Ok(new { Message = "Updated Successfully" });

        }

        
        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAssignment([FromRoute] int id)
        {
            var assignmentDomainModel = await _unitOfWork.Assignment.GetByIdWithIncludeAsync(id);

            if (assignmentDomainModel == null)
            {
                return NotFound(new { Message = $"Assignment wit id: {id} not found" });
            }

            if (!await _unitOfWork.Assignment.DeleteAssignmentAsync(assignmentDomainModel))
            {
                return BadRequest(new { Mesaage = "Somthing went wrong while deleting" });
            }

            return Ok(new { Message = "Deleted Successfully" });
        }

        [HttpGet]
        [Route("ByAssignmentId/{assignmentId:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetAssignmentById([FromRoute] int assignmentId)
        {
            var assignmentDomainModel = await _unitOfWork.Assignment.GetByIdWithIncludeAsync(assignmentId);
            if (assignmentDomainModel == null)
            {
                return NotFound(new { Message = "assignment not found" });
            }

            return Ok(_mapper.Map<AssignmentDto>(assignmentDomainModel));
        }
    }

}
