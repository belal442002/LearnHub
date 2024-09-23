using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.InstructorDto;
using LearnHub.API.Models.Dto.ParentDto;
using LearnHub.API.Models.Dto.StudentDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class InstructorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstructorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInstructors([FromQuery] bool? archive)
        {
            var InstructorDomainModel = await _unitOfWork.Instructor.GetAllInstructorsWithAccountAsync(archive ?? true);
            return Ok(_mapper.Map<List<InstructorDto>>(InstructorDomainModel));
        }

        [HttpGet]
        [ValidationModel]
        [Route("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var instructorDomainModel = await _unitOfWork.Instructor.GetInstructorByIdWithAccountAsync(id);

            if (instructorDomainModel == null)
            {
                return NotFound(new { Message = "Instructor Not found" });
            }

            return Ok(_mapper.Map<InstructorDto>(instructorDomainModel));
        }

        [HttpGet]
        [Route("ByCourseId/{courseId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInstructorsByCourseId([FromRoute] String courseId, 
            [FromQuery]int? semesterId, [FromQuery]int? year)
        {
            var courseDomainModel = await _unitOfWork.Course.GetByIdAsync(courseId);
            if (courseDomainModel == null)
            {
                return NotFound(new { Message = "Course not found" });
            }
            var teachesDomainModel = await _unitOfWork.Teach
                .GetTeachesByCourseIdAsync(courseId, semesterId, year);

            if(teachesDomainModel.Count() == 0)
            {
                return NotFound(new { Message = "No instructors teach this course" });
            }

            return Ok(_mapper.Map<List<InstructorByCourseDto>>(teachesDomainModel));
        }

        [HttpDelete]
        [ValidationModel]
        [Route("Archive/{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveInstructor([FromRoute] int id)
        {
            var instructorDomainModel = await _unitOfWork.Instructor.GetByIdAsync(id);

            if (instructorDomainModel == null)
            {
                return NotFound(new { Message = "Instructor not found" });
            }

            // archive student
            await _unitOfWork.Instructor.Archive(instructorDomainModel);
            return Ok(new { Message = "Instructor Archived" });
        }

        [HttpPut]
        [ValidationModel]
        [Route("UpdateInstructor/{id:int}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> UpdateInstructorById([FromRoute] int id,
            [FromBody] UpdateInstructorRequestDto updateInstructorRequest)
        {
            try
            {
                var instructorDomainModel = _mapper.Map<Instructor>(updateInstructorRequest);
                var updatedInstructor = await _unitOfWork.Instructor.UpdateInstructorAsync(id, instructorDomainModel);
                if (!updatedInstructor)
                {
                    return BadRequest(new { Message = "Something went wrong while updating" });
                }
                return Ok(new { Message = "Instructor updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}