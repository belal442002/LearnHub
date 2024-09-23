using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.StudentDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudents([FromQuery] bool? archive)
        {
            var studentsDomainModel = await _unitOfWork.Student.GetAllStudentsWithAccountAsync(archive ?? true);
            return Ok(_mapper.Map<List<StudentDto>>(studentsDomainModel));
        }

        [HttpGet]
        [Route("ByCourseId/{courseId}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetStudentsByCourseId([FromRoute] String courseId,
            [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect course id" });
            }

            var studentCoursesDomainModel = await _unitOfWork.StudentCourse
                .GetStudentCoursesByCourseIdAsync(courseId, semesterId, year);

            return Ok(_mapper.Map<List<StudentByCourseDto>>(studentCoursesDomainModel));
        }

        [HttpGet]
        [ValidationModel]
        [Route("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var studentDomainModel = await _unitOfWork.Student.GetStudentByIdWithAccountAsync(id);

            if (studentDomainModel == null)
            {
                return NotFound(new { Message = "Student not found" });
            }

            return Ok(_mapper.Map<StudentDto>(studentDomainModel));
        }

        [HttpDelete]
        [ValidationModel]
        [Route("Archive/{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveStudent([FromRoute] int id)
        {
            var studentDomainModel = await _unitOfWork.Student.GetByIdAsync(id);

            if (studentDomainModel == null)
            {
                return NotFound(new { Message = "Student not found" });
            }

            // archive student
            await _unitOfWork.Student.Archive(studentDomainModel);
            return Ok(new { Message = "student Archived" });
        }
        [HttpPut]
        [ValidationModel]
        [Route("UpdateStudent/{id:int}")]
        //[Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> UpdateStudentById([FromRoute] int id, 
            [FromBody] UpdateStudentRequestDto updateStudentRequestDto)
        {
            try
            {
                var studentDomainModel = _mapper.Map<Student>(updateStudentRequestDto);
                var updatedStudent = await _unitOfWork.Student.UpdateStudentAsync(id, studentDomainModel);
                if (!updatedStudent)
                {
                    return BadRequest(new { Message = "Something went wrong while updating" });
                }
                return Ok(new { Message = "Student updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}