using AutoMapper;
using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto;
using LearnHub.API.Models.Dto.AnnouncementDto;
using LearnHub.API.Models.Dto.CourseDto;
using LearnHub.API.Models.Dto.DifficultyDto;
using LearnHub.API.Models.Dto.InstructorDto;
using LearnHub.API.Models.Dto.MaterialDto;
using LearnHub.API.Models.Dto.MaterialTypeDto;
using LearnHub.API.Models.Dto.QBAnswersDto;
using LearnHub.API.Models.Dto.QuestionBankDto;
using LearnHub.API.Models.Dto.QuestionTypeDto;
using LearnHub.API.Models.Dto.StudentCourseDto;
using LearnHub.API.Models.Dto.StudentDto;
using LearnHub.API.Models.Dto.TeachDto;
using LearnHub.API.Models.Dto.TopicDto;
using Microsoft.AspNetCore.Identity;
using MaterialType = LearnHub.API.Models.Domain.MaterialType;
using QuestionType = LearnHub.API.Models.Domain.QuestionType;
using MaterialDto = LearnHub.API.Models.Dto.MaterialDto.MaterialDto;
using LearnHub.API.Models.Dto.AssignmentDto;
using LearnHub.API.Models.Dto.EvaluationDto;
using LearnHub.API.Models.Dto.ParentDto;

namespace LearnHub.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student, Instructor, Parent
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<UpdateStudentRequestDto, Student>().ReverseMap();
            CreateMap<UpdateInstructorRequestDto, Instructor>().ReverseMap();
            CreateMap<UpdateParentRequestDto, Parent>().ReverseMap();


            CreateMap<IdentityUser, AccountDto>().ReverseMap();
            CreateMap<IdentityUser, UpdateAccountDto>().ReverseMap();
            CreateMap<Instructor, InstructorDto>().ReverseMap();
            CreateMap<Parent, ParentDto>().ReverseMap();

            


            // QuestionBank
            CreateMap<QuestionBank, QuestionBankDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuestionType.Name))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty.Name))
                .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Topic.TopicName));

            CreateMap<QuestionBank, QuestionBankForDifficultyAndQuestionTypeDTO>()
                .ForMember(dest => dest.DifficultyName, opt => opt.MapFrom(src => src.Difficulty.Name))
                .ForMember(dest => dest.QuestionTypeName, opt => opt.MapFrom(src => src.QuestionType.Name))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));

            CreateMap<AddQuestionBankRequestDto, QuestionBank>().ReverseMap();
            CreateMap<AddQBAnswerRequestDto, QBAnswers>().ReverseMap();
            CreateMap<QBAnswersDto, QBAnswers>().ReverseMap();
            //new
            CreateMap<UpdateQuestionBankRequestDto, QuestionBank>().ReverseMap();
            CreateMap<UpdateQBAnswersRequestDto, QBAnswers>().ReverseMap();



            // Course
            CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.Teaches.Select(t => t.Instructor)))
            .ForMember(dest => dest.QuestionBanks, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials))
            .ForMember(dest => dest.Announcements, opt => opt.MapFrom(src => src.Announcements));
            CreateMap<CreateCourseDTO, Course>().ReverseMap();
            CreateMap<UpdateCourseDTO, Course>().ReverseMap();
            CreateMap<CourseDtoForTopic, Course>().ReverseMap();
            CreateMap<SimpleCourseDto, Course>().ReverseMap();
            CreateMap<CourseWithCodeDto, Course>().ReverseMap();

            // Teach
            CreateMap<Teach, InstructorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Instructor.Name));

            CreateMap<AddTeachRequestDto, Teach>()
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => SystemService.GetSemesterByMonth()));

            CreateMap<Teach, TeachDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            CreateMap<Teach, CourseByInstructorDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            CreateMap<Teach, InstructorByCourseDto>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            // Announcement
            CreateMap<Announcement, AnnouncementDTO>()
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name));
            CreateMap<Announcement, AddAnnouncementDto>().ReverseMap();
            CreateMap<Announcement, UpdateAnnoncementRequestDto>().ReverseMap();

            
            // Topic
            CreateMap<CreateTopicDto, Topic>().ReverseMap();
            CreateMap<Topic, TopicDTO>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course));
            CreateMap<UpdateTopicRequestDto, Topic>().ReverseMap();

            // Difficulty, QuestionType, MaterialType
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<QuestionType, QuestionTypeDTO>().ReverseMap();
            CreateMap<Material, MaterialDto>().ReverseMap();
            CreateMap<MaterialType, MaterialTypeDTO>().ReverseMap();

            //StudentCourse
            CreateMap<AddStudentCourseRequestDto, StudentCourse>()
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src => SystemService.GetSemesterByMonth()));

            CreateMap<StudentCourse, StudentCourseDto>()
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            CreateMap<StudentCourse, CourseByStudentDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            CreateMap<StudentCourse, StudentByCourseDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.SemesterName));

            // Material
            CreateMap<Material, MaterialDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.MaterialType, opt => opt.MapFrom(src => src.MaterialType.Name));

            CreateMap<AddMaterialRequestDto, Material>();

            // Assignment
            CreateMap<AssignmentConfig, AddAssignmentConfigRequestDto>().ReverseMap();

            CreateMap<AssignmentConfigTopic, AddAssignmentConfigTopicsRequestDto>().ReverseMap();

            CreateMap<Assignment, AssignmentDto>().ReverseMap();

            CreateMap<AssignmentConfig, AssignmentConfigDto>().ReverseMap();

            CreateMap<UpdateAssignmentRequestDto, Assignment>().ReverseMap();

            CreateMap<UpdateAssignmentConfigRequestDto, AssignmentConfig>().ReverseMap();

            CreateMap<UpdateAssignmentConfigTopicRequestDto, AssignmentConfigTopic>().ReverseMap();

            CreateMap<AssignmentQuestion, LearnHub.API.Models.Dto.AssignmentQuestionDto.AssignmentQuestionDto>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ReverseMap();

            CreateMap<AssignmentConfigTopic, AssignmentConfigTopicsDto>()
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName))
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.Difficulty.Name))
                .ReverseMap();


            // Evaluation
            CreateMap<Evaluation, EvaluationDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Assignment.AssignmentConfig.Title))
                .ForMember(dest => dest.MaxScore, opt => opt.MapFrom(src => src.Assignment.AssignmentConfig.MaxScore))
                .ForMember(dest => dest.AssignmentNumber, opt => opt.MapFrom(src => src.Assignment.AssignmentNumber))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Assignment.AssignmentConfig.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Assignment.AssignmentConfig.EndDate))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => SystemService.GetDurationInMinutes(
                    src.Assignment.AssignmentConfig.EndDate, src.Assignment.AssignmentConfig.StartDate)))
                .ReverseMap();
                
        }
    }
}
