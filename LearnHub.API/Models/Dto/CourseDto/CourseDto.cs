using LearnHub.API.Models.Dto.AnnouncementDto;

namespace LearnHub.API.Models.Dto.CourseDto
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Models.Dto.InstructorDto.InstructorDto> Instructors { get; set; }
        public ICollection<Models.Dto.QuestionBankDto.QuestionBankDto> QuestionBanks { get; set; }
        public ICollection<LearnHub.API.Models.Dto.MaterialDto.MaterialDto> Materials { get; set; }
        public ICollection<AnnouncementDTO> Announcements { get; set; }
    }
}
