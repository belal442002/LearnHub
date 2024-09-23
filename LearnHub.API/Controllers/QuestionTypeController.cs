using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Dto.DifficultyDto;
using LearnHub.API.Models.Dto.QuestionTypeDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class QuestionTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetQuestionTypes()
        {
            var questionTypes = await _unitOfWork.QuestionType.GetQuestionTypesWithQuestionsAsync();
            return Ok(_mapper.Map<List<QuestionTypeDTO>>(questionTypes));
        }

        [HttpGet("{id}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetQuestionTypeById(int id)
        {
            var questionType = await _unitOfWork.QuestionType.GetQuestionTypeByIdWithQuestionsAsync(id);
            if (questionType == null)
            {
                return NotFound(new { Message = "Questoin type not found" });
            }

            return Ok(_mapper.Map<QuestionTypeDTO>(questionType));
        }

        [HttpGet("name/{name}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetQuestionTypeByName(string name)
        {
            var questionType = await _unitOfWork.QuestionType.GetQuestionTypeByNameWithQuestionsAsync(name);
            if (questionType == null)
            {
                return NotFound(new { Message = "Questoin type not found" });
            }

            return Ok(_mapper.Map<QuestionTypeDTO>(questionType));
        }
    }
}