using AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.MaterialDto;
using LearnHub.API.UniteOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        public MaterialController(IUnitOfWork unitOfWork, IMapper mapper, FileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> GetAllMaterials()
        {
            var materialsDomainModel = await _unitOfWork.Material.GetAllWithIncludeAsync();
            return Ok(_mapper.Map<List<MaterialDto>>(materialsDomainModel));
        }

        [HttpGet("{courseId}")]
        //[Authorize(Roles = "Instructor, Student")]
        public async Task<IActionResult> GetAllMaterialsByCourseId(string courseId, [FromQuery] int? materialTypeId)
        {
            if (await _unitOfWork.Course.GetByIdAsync(courseId) == null)
            {
                return NotFound(new { Message = "courseId not found" });
            }
            var materialsDomainModel = await _unitOfWork.Material.GetAllByCourseIdAsync(courseId, materialTypeId);
            return Ok(_mapper.Map<List<MaterialDto>>(materialsDomainModel));
        }

        [HttpPost]
        [ValidationModel]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddMaterial([FromQuery] AddMaterialRequestDto materialDto)
        {
            var courseDomainModel = await _unitOfWork.Course.GetByIdAsync(materialDto.CourseId);
            if (courseDomainModel == null)
            {
                return NotFound(new { Message = "Course not found" });
            }
            if (await _unitOfWork.MaterialType.GetByIdAsync(materialDto.MaterialTypeId) == null)
            {
                return NotFound(new { Message = "Material type Id not found" });
            }
            try
            {
                var materialLink = await _fileService.UploadFileAsync(materialDto.File);

                var materialDomainModel = _mapper.Map<Material>(materialDto);
                materialDomainModel.MaterialLink = _fileService.GenerateDownloadLink(materialLink);
                materialDomainModel.MaterialPath = materialLink;
                if (!await _unitOfWork.Material.AddAsync(materialDomainModel))
                {
                    ModelState.AddModelError("", "Something went wrong while adding material");
                    return StatusCode(500, ModelState);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message});
            }

            return Ok(new { Message = "Material Added successfully" });
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteMaterialById(int id)
        {
            var material = await _unitOfWork.Material.GetByIdAsync(id);
            if (material == null)
            {
                return NotFound(new { Message = "Material not found" });
            }
            try
            {
                if (!_fileService.DeleteFile(material.MaterialPath))
                {
                    return NotFound(new { Message = material.MaterialLink });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message});
            }

            await _unitOfWork.Material.DeleteAsync(material);

            return Ok(new { Message = "Material Deleted Successfully" });
        }

        [HttpDelete("DeleteByCourse/{courseId}")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteMaterialsByCourse(string courseId, [FromQuery] int? materialTypeId)
        {
            var course = await _unitOfWork.Course.GetByIdAsync(courseId);
            if (course == null)
            {
                return NotFound(new { Message = "Course not found" });
            }
            var materials = await _unitOfWork.Material.GetAllByCourseIdAsync(courseId, materialTypeId);

            if (!materials.Any())
            {
                return NotFound(new { Message = "No materials found for this course" });
            }

            try
            {
                foreach (var material in materials)
                {
                    if (!_fileService.DeleteFile(material.MaterialPath))
                    {
                        return NotFound(new { Message = material.MaterialLink });
                    }
                    await _unitOfWork.Material.DeleteAsync(material);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new { Message = "Materials Deleted Successfully" });
        }
    }
}
