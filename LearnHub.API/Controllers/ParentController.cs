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
    public class ParentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllParents([FromQuery] bool? archive)
        {
            var parentsDomainModel = await _unitOfWork.Parent.GetAllParentsWithAccountAsync(archive ?? true);
            return Ok(_mapper.Map<List<ParentDto>>(parentsDomainModel));
        }

        [HttpGet]
        [ValidationModel]
        [Route("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetParentById([FromRoute] int id)
        {
            var parentDomainModel = await _unitOfWork.Parent.GetParentByIdWithAccountAsync(id);

            if (parentDomainModel == null)
            {
                return NotFound(new { Message = "Parent not found" });
            }

            return Ok(_mapper.Map<ParentDto>(parentDomainModel));
        }

        [HttpDelete]
        [ValidationModel]
        [Route("Archive/{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveParent([FromRoute] int id)
        {
            var parentDomainModel = await _unitOfWork.Parent.GetByIdAsync(id);

            if (parentDomainModel == null)
            {
                return NotFound(new { Message = "Parent not found" });
            }

            // archive parent
            await _unitOfWork.Parent.Archive(parentDomainModel);
            return Ok(new { Message = "Parent Archived" });
        }

        [HttpPost]
        [ValidationModel]
        [Route("LinkStudent")]
        //[Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> LinkStudentToParent([FromBody] LinkStudentToParentDto linkDto)
        {
            var parentDomainModel = await _unitOfWork.Parent.GetByIdAsync(linkDto.ParentId);
            var studentDomainModel = await _unitOfWork.Student.GetByIdAsync(linkDto.StudentId);

            if (parentDomainModel == null || studentDomainModel == null)
            {
                return NotFound(new { Message = "Parent or Student not found" });
            }

            studentDomainModel.ParentId = linkDto.ParentId;

            var result = await _unitOfWork.Student.UpdateAsync(studentDomainModel);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error linking student to parent");
            }

            return Ok(new { Message = "Student linked to parent successfully" });
        }

        [HttpGet]
        [ValidationModel]
        [Route("ParentByStudent/{studentId:int}")]
        //[Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> GetParentByStudentId([FromRoute] int studentId)
        {
            var parentDomainModel = await _unitOfWork.Parent.GetParentByStudentIdAsync(studentId);

            if (parentDomainModel == null)
            {
                return NotFound(new { message = "Parent not found for the given student ID" });
            }

            return Ok(_mapper.Map<ParentDto>(parentDomainModel));
        }

        [HttpPut]
        [ValidationModel]
        [Route("UpdateParent/{id:int}")]
        //[Authorize(Roles = "Parent, Admin")]
        public async Task<IActionResult> UpdateParentById([FromRoute] int id,
            [FromBody] UpdateParentRequestDto updateParentRequest)
        {
            try
            {
                var parentDomainModel = _mapper.Map<Parent>(updateParentRequest);
                var updatedParent = await _unitOfWork.Parent.UpdateParentAsync(id, parentDomainModel);
                if (!updatedParent)
                {
                    return BadRequest(new { Message = "Something went wrong while updating" });
                }
                return Ok(new { Message = "Parent updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [ValidationModel]
        [Route("GetStudentsByParentId/{parentId:int}")]
        public async Task<IActionResult> GetStudentsByParentId([FromRoute] int parentId)
        {
            var parentDomainModel = await _unitOfWork.Parent.GetByIdAsync(parentId);

            if (parentDomainModel == null)
            {
                return NotFound(new { Message = "Parent not found" });
            }
            var studentsDomainModel = await _unitOfWork.Parent.GetStudentsByParentId(parentId);
            return Ok(_mapper.Map<List<StudentDto>>(studentsDomainModel));
        }
    }
}