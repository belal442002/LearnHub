using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AnnouncementDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AnnouncementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnnouncementController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles= "Instructor")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AddAnnouncementDto announcementDto)
        {
            if (await _unitOfWork.Instructor.GetByIdAsync(announcementDto.InstructorId) == null)
            {
                return NotFound(new { Message = "Instructor Id not found" });
            }
            if (await _unitOfWork.Course.GetByIdAsync(announcementDto.CourseId) == null)
            {
                return NotFound(new { Message = "Course Id not found" });
            }

            var teaches = await _unitOfWork.Teach
                .GetAllWithIncludeAsync(announcementDto.InstructorId
                , announcementDto.CourseId, SystemService.GetSemesterByMonth(), DateTime.UtcNow.Year);

            if (teaches.Count() == 0)
            {
                return NotFound( new { Message = "Instructor has no access on this course"});
            }
            var announcement = _mapper.Map<Announcement>(announcementDto);

            var added = await _unitOfWork.Announcement.AddAsync(announcement);

            if (!added)
            {
                return BadRequest(new { Message = "Failed to create announcement. Please try again." });
            }

            return Ok(new { Message = "Announcement added successfully" });
        }

        [HttpGet]
        [Route("{courseId}")]
        //[Authorize(Roles = "Instructor, Student, Parent")]
        public async Task<IActionResult> GetAll([FromRoute] String courseId)
        {
            if(courseId.IsNullOrEmpty() || await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { message = $"There is no course with id: {courseId}"}); 
            }
            return Ok(_mapper.Map<List<AnnouncementDTO>>(await _unitOfWork.Announcement.GetAllAnnouncementWithIncludeAsync(courseId)));
        }


        [HttpPut]
        [Route("{announcementId:int}")]
        [ValidationModel]
        public async Task<IActionResult> UpdateAnnouncement([FromRoute]int announcementId, [FromBody] UpdateAnnoncementRequestDto updateAnnoncement)
        {
            var announcementDomainModel = _mapper.Map<Announcement>(updateAnnoncement);

            try
            {
                if(!await _unitOfWork.Announcement.UpdateAnnouncementAsync(announcementId, announcementDomainModel))
                {
                    return BadRequest(new { Message = "Somthing went wrong while updating announcement" });
                }

                return Ok(new { Message = "Updated successfully" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcementDomainModel = await _unitOfWork.Announcement.GetByIdAsync(id);

            if(announcementDomainModel == null)
            {
                return NotFound(new { Message = $"No Announcement exist with id {id}" });
            }

            if(!await _unitOfWork.Announcement.DeleteAsync(announcementDomainModel))
            {
                return BadRequest(new { Message = "Somthing went wrong while deleting announcement" });
            }

            return Ok( new { Message = "Deleted successfully" });
        }

    }
}
