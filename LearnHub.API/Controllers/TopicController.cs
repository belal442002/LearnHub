using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AnnouncementDto;
using LearnHub.API.Models.Dto.TopicDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TopicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TopicController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddTopic([FromBody] CreateTopicDto topicDto)
        {
            if(await _unitOfWork.Course.GetByIdAsync(topicDto.CourseId) == null)
            {
                return NotFound(new { Message = "Course not found" });
            }
            var topic = _mapper.Map<Topic>(topicDto);

            var added = await _unitOfWork.Topic.AddAsync(topic);

            if (!added)
            {
                return BadRequest(new { Message = "Failed to add topic. Please try again." });
            }

            return Ok(new { Message = "Topic added successfully" });
        }

        [HttpGet("{courseId}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetTopicsByCourseId(string courseId)
        {
            if (await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "Course not found" });
            }
            var topics = await _unitOfWork.Topic.GetTopicsByCourseIdAsync(courseId);

            if (!topics.Any())
            {
                return NotFound(new { Message = "This course have not topics yet" });
            }

            return Ok(_mapper.Map<IEnumerable<TopicDTO>>(topics));
        }


        [HttpPut]
        [Route("UpdateTopic/{topicId:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateTopic([FromRoute] int topicId, [FromBody] UpdateTopicRequestDto updateTopic)
        {
            try
            {
                var topicDomainModel = _mapper.Map<Topic>(updateTopic);
                if (!await _unitOfWork.Topic.UpdateTopicAsync(topicId, topicDomainModel))
                {
                    return BadRequest(new { Message = "Something went wrong while updating" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            return Ok(new { Message = "Updated successfully" });
        }

        [HttpDelete]
        [Route("DeleteTopic/{topicId:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteTopic([FromRoute] int topicId)
        {
            try
            {
                if(!await _unitOfWork.Topic.DeleteTopicAsync(topicId))
                {
                    return BadRequest(new { Message = "Something went wrong while deleting" });
                }

            } catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            return Ok(new { Message = "Deleted successfully"});
        }

    }
}
