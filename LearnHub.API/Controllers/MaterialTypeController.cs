using AutoMapper;
using LearnHub.API.Models.Dto.MaterialTypeDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MaterialTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetAllMaterialTypes()
        {
            var materialTypes = await _unitOfWork.MaterialType.GetAllMaterialTypesAsync();
            return Ok(_mapper.Map<List<MaterialTypeDTO>>(materialTypes));
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetMaterialTypeById(int id)
        {
            var materialType = await _unitOfWork.MaterialType.GetMaterialTypeByIdAsync(id);
            if (materialType == null)
            {
                return NotFound(new { Mesaage = "Material type not found" });
            }

            return Ok(_mapper.Map<MaterialTypeDTO>(materialType));
        }

        [HttpGet("name/{name}")]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetMaterialTypeByName(string name)
        {
            var materialType = await _unitOfWork.MaterialType.GetMaterialTypeByNameAsync(name);
            if (materialType == null)
            {
                return NotFound(new { Message = "Material type not found" });
            }

            return Ok(_mapper.Map<MaterialTypeDTO>(materialType));
        }

    }
}
