using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.TeachDto;
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
    public class TeachController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeachController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> Add([FromQuery]AddTeachRequestDto teachRequestDto)
        {
            if(await _unitOfWork.Instructor.GetByIdAsync(teachRequestDto.InstructorId) == null)
            {
                return NotFound(new { Message = "Incorrect instructor id" });
            }
            if(await _unitOfWork.Course.GetByIdAsync(teachRequestDto.CourseId) == null)
            {
                return NotFound(new { Message = "Incorrect courseId" });
            }

            var teachDomainModel = _mapper.Map<Teach>(teachRequestDto);

            if (await _unitOfWork.Teach.ExistAsync(teachDomainModel))
            {
                return BadRequest(new { Message = "This instructor already registered in this course" });
            }

            if(!await _unitOfWork.Teach.AddAsync(teachDomainModel))
            {
                ModelState.AddModelError("", "Somthing went wrong while register instructor in the course");
                return StatusCode(500, ModelState);
            }

            return Ok(new { Message = "Registered successfully" });
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery]int? instructorId, [FromQuery]String? courseId,
            [FromQuery]int? semesterId, [FromQuery] int? year)
        {
            if (instructorId != null && await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Incorrect instructor id" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect courseId" });
            }
            var teachesDomainModel = await _unitOfWork.Teach
               .GetAllWithIncludeAsync(instructorId, courseId, semesterId, year);

            return Ok(_mapper.Map<List<TeachDto>>(teachesDomainModel));
        }

        [HttpDelete]
        [Route("RemoveAll/{instructorId:int}/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAll([FromRoute] int instructorId, [FromRoute] String courseId,
            [FromQuery] int? semesterId, [FromQuery] int? year)
        {
            if (await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Incorrect instructorId" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect courseId" });
            }

            var teachesDomainModel = await _unitOfWork.Teach
                .GetAllWithIncludeAsync(instructorId, courseId, semesterId, year);

            if (!await _unitOfWork.Teach.DeleteRangeAsync(teachesDomainModel.ToList()))
            {
                ModelState.AddModelError("", "Somthing went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpDelete]
        [Route("{instructorId:int}/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByInstructorAndCourseId([FromRoute] int instructorId, [FromRoute] String courseId)
        {
            if (await _unitOfWork.Instructor.GetByIdAsync(instructorId) == null)
            {
                return NotFound(new { Message = "Incorrect instructorId" });
            }
            if (!courseId.IsNullOrEmpty() && await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Incorrect courseId" });
            }

            var teachDomainModel = await _unitOfWork.Teach
                .GetByInstructorAndCourseIdAsync(instructorId, courseId);

            if(teachDomainModel == null)
            {
                return NotFound();
            }

            if (!await _unitOfWork.Teach.DeleteAsync(teachDomainModel))
            {
                ModelState.AddModelError("", "Somthing went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new { Message = "Deleted successfully" });
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {

            var teachDomainModel = await _unitOfWork.Teach.GetByIdAsync(id);

            if(teachDomainModel == null)
            {
                return NotFound(new { Message = "Not found" });
            }

            if(!await _unitOfWork.Teach.DeleteAsync(teachDomainModel))
            {
                ModelState.AddModelError("", "Somthing went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok(new { Message = "Deleted successfully" });
        }
    }
}
