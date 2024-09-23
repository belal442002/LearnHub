using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Dto.DifficultyDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DifficultyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DifficultyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetDifficulties()
        {
            var difficulties = await _unitOfWork.Difficulty.GetDifficultiesWithQuestionsAsync();
            return Ok(_mapper.Map<List<DifficultyDTO>>(difficulties));
        }

        [HttpGet("{id}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetDifficultyById(int id)
        {
            var difficulty = await _unitOfWork.Difficulty.GetDifficultyByIdWithQuestionsAsync(id);
            if (difficulty == null)
            {
                return NotFound(new { Message = "No difficulty wiht this Id" });
            }

            return Ok(_mapper.Map<DifficultyDTO>(difficulty));
        }

        [HttpGet("name/{name}")]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetDifficultyByName(string name)
        {
            var difficulty = await _unitOfWork.Difficulty.GetDifficultyByNameWithQuestionsAsync(name);
            if (difficulty == null)
            {
                return NotFound(new { Message = "No difficulty with this name" });
            }
            return Ok(_mapper.Map<DifficultyDTO>(difficulty));
        }
    }
}