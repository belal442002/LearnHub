using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.StudentCourseDto;
using LearnHub.API.Models.Dto.StudentDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentCourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentCourseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> AddStudentCourse([FromBody] AddStudentCourseRequestDto studentCourse)
        {
            if(await _unitOfWork.Student.GetByIdAsync(studentCourse.StudentId) == null)
            {
                return NotFound(new { Message = "student id not found" });
            }
            var course = await _unitOfWork.Course.GetByIdAsync(studentCourse.CourseId);
            if (course == null)
            {
                return NotFound(new { Message = "course id not found" });
            }
            if (course.CourseCode != studentCourse.CourseCode)
            {
                return BadRequest(new { Message = "Incorrect course code "});
            }

            var studentCourseDomainModel = _mapper.Map<StudentCourse>(studentCourse);

            if(await _unitOfWork.StudentCourse.ExistAsync(studentCourseDomainModel))
            {
                return BadRequest(new { message = "You have already registered in this course" });
            }

            if(!await _unitOfWork.StudentCourse.AddAsync(studentCourseDomainModel))
            {
                ModelState.AddModelError("", "Somthing went wrong while registering the student to this course");
                return StatusCode(500, ModelState);
            }
            return Ok(new { Message = "Registered successfully" });
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int? studentId,
            [FromQuery] String? courseId, [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if (studentId != null && await _unitOfWork.Student.GetByIdAsync(studentId) == null)
            {
                return NotFound(new {Message = "Incorrect student id" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect course id" });
            }

            var studentCoursesDomainModel = await _unitOfWork.StudentCourse
                .GetAllStudentCoursesWithIncludeAsync(studentId, courseId, semesterId, year);

            return Ok(_mapper.Map<List<StudentCourseDto>>(studentCoursesDomainModel));
        }

        [HttpDelete]
        [Route("RemoveAll/{studentId:int}/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAll([FromRoute] int studentId,
            [FromRoute] String courseId, [FromQuery] int? semesterId, int? year)
        {
            if (await _unitOfWork.Student.GetByIdAsync(studentId) == null)
            {
                return NotFound(new { Message = "Incorrect student id" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect course id" });
            }

            var studentCoursesDomainModel = await _unitOfWork.StudentCourse
                .GetAllStudentCoursesWithIncludeAsync(studentId, courseId, semesterId, year);

            if (studentCoursesDomainModel.Count() == 0)
            {
                return NotFound();
            }

            await _unitOfWork.StudentCourse.DeleteRangeAsync(studentCoursesDomainModel.ToList());
            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpDelete]
        [Route("{studentId:int}/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByStudentAndCourseId([FromRoute] int studentId, [FromRoute] String courseId)
        {
            if (await _unitOfWork.Student.GetByIdAsync(studentId) == null)
            {
                return NotFound(new { Message = "Incorrect student id" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect course id" });
            }

            var studentCourseDomainModel = await _unitOfWork.StudentCourse
                .GetByStudentAndCourseIdAsync(studentId, courseId);

            if (studentCourseDomainModel == null)
            {
                return NotFound();
            }

            await _unitOfWork.StudentCourse.DeleteAsync(studentCourseDomainModel);

            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var studentCourseDomainModel = await _unitOfWork.StudentCourse
                .GetByIdAsync(id);

            if (studentCourseDomainModel == null)
            {
                return NotFound(new { Message = "Not found" });
            }

            await _unitOfWork.StudentCourse.DeleteAsync(studentCourseDomainModel);

            return Ok(new { Message = "Deleted successfully" });
        }
    }
}
