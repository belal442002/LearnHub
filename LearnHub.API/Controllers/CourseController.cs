using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.CourseDto;
using LearnHub.API.Models.Dto.InstructorDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCourses()
        {
            //var coursesDomainModel = await _unitOfWork.Course.GetAllCoursesWithIncludeAsync();
            //var coursesDomainModel = await _unitOfWork.Course.GetAllCoursesAsync();
            //var courseDTOs = _mapper.Map<IEnumerable<SimpleCourseDto>>(coursesDomainModel);
            //return Ok(courseDTOs);

            var coursesDomainModel = await _unitOfWork.Course.GetAllCoursesAsync();
            var courseDTOs = _mapper.Map<IEnumerable<CourseWithCodeDto>>(coursesDomainModel);
            return Ok(courseDTOs);

        }

        [HttpGet("{id}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCourseById(string id)
        {
            //var courseDomainModel = await _unitOfWork.Course.GetCourseByIdWithIncludeAsync(id);
            //var courseDomainModel = await _unitOfWork.Course.GetByIdAsync(id);
            //if (courseDomainModel == null)
            //{
            //    return NotFound(new { Message = "Course not found" });
            //}

            //var courseDTO = _mapper.Map<SimpleCourseDto>(courseDomainModel);
            //return Ok(courseDTO);

            var courseDomainModel = await _unitOfWork.Course.GetByIdAsync(id);
            if (courseDomainModel == null)
            {
                return NotFound(new { Message = "Course not found" });
            }

            var courseDTO = _mapper.Map<CourseWithCodeDto>(courseDomainModel);
            return Ok(courseDTO);
        }

        [HttpGet]
        [Route("CoursesByStudentId/{studentId:int}")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetCoursesByStudentId([FromRoute] int studentId,
            [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if (await _unitOfWork.Student.GetByIdAsync(studentId) == null)
            {
                return NotFound(new { Message = "Incorrect student id" });
            }

            var studentCoursesDomainModel = await _unitOfWork.StudentCourse
                .GetStudentCoursesByStudentIdAsync(studentId, semesterId, year);

            return Ok(_mapper.Map<List<CourseByStudentDto>>(studentCoursesDomainModel));
        }

        [HttpGet]
        [Route("CoursesByInstructorId/{instructorId:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetCoursesByInstructorId([FromRoute] int instructorId,
            [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if (await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Inccorect Instructor Id" });
            }
            var teachesDomainModel = await _unitOfWork.Teach
                .GetTeachesByInstructorIdAsync(instructorId, semesterId, year);

            if (teachesDomainModel.Count() == 0)
            {
                return NotFound(new { Message = "Instructor does not teach any course" });
            }

            return Ok(_mapper.Map<List<CourseByInstructorDto>>(teachesDomainModel));
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO createCourseDTO)
        {
            var course = _mapper.Map<Course>(createCourseDTO);
            var added = await _unitOfWork.Course.AddAsync(course);

            if (!added)
            {
                return BadRequest(new { Message = "Failed to create course. Please try again." });
            }

            var courseDTO = _mapper.Map<CourseDto>(course);
            return Ok(new { Message = "Course created successfully" });
        }

        [HttpGet]
        [Route("StudentCount")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetCourseWithStudentCount([FromQuery] int? semesterId, [FromQuery] int? year)
        {
            return Ok(await _unitOfWork.Course.GetCourseWithStudentCountAsync(semesterId, year));
        }

        [HttpGet]
        [Route("StudentCountForCourse/{courseId}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetSpeceficCourseWithStudentCount([FromRoute]String courseId, [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if(await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = $"No course with id {courseId}" });
            }

            return Ok(await _unitOfWork.Course.GetSpeceficCourseWithStudentCountAsync(courseId, semesterId, year));
        }
    }
}