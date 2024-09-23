using AutoMapper;
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
    public class EvaluationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EvaluationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{courseId}")]
        //[Authorize(Roles = "Instructor, Student, Parent")]
        public async Task<IActionResult> GetALlEvaluations([FromRoute]string courseId, 
            [FromQuery]int? assignmentId, 
            [FromQuery] int? studentId, [FromQuery] int? assignmentNumber)
        {
            var courseDomainModel = await _unitOfWork.Course.GetByIdAsync(courseId);
            if (courseDomainModel == null)
            {
                return NotFound(new { Message = "Course not found" });
            }

            var evaluations = await _unitOfWork.Evaluation
                .GetAllEvaluationsAsync(courseId, assignmentId, studentId, assignmentNumber);

            return Ok(_mapper.Map<List<EvaluationDto>>(evaluations));
        }
    }
}
