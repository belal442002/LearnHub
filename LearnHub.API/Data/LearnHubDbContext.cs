using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LearnHub.API.Data
{
    public class LearnHubDbContext : IdentityDbContext
    {
        public LearnHubDbContext(DbContextOptions<LearnHubDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Models.Domain.MaterialType> MaterialTypes { get; set; }
        public DbSet<QBAnswers> QBAnswers { get; set; }
        public DbSet<QuestionBank> Questions { get; set; }
        public DbSet<Models.Domain.QuestionType> QuestionTypes { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Teach> Teaches { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Assignment> Assignments { get; set; }  
        public DbSet<AssignmentConfig> AssignmentConfigs { get; set; }  
        public DbSet<AssignmentConfigTopic> AssignmentConfigTopics { get; set; }
        public DbSet<AssignmentQuestion> AssignmentQuestions { get; set; }  
        public DbSet<Evaluation> Evaluations { get; set; }  
        public DbSet<Parent> Parents { get; set; }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seeding roles into the database
            var studentRoleId = "26c2b87c-bd59-48aa-87b6-34b414f8d12e";
            var instructorRoleId = "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d";
            var adminRoleId = "b985b240-2dce-4365-bcf5-c4c792b9076b";
            //new
            var parentRoleId = "3125b3a6-1b98-4d83-b473-8a6f4dd73f53";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = studentRoleId,
                    ConcurrencyStamp = studentRoleId,
                    Name = Helper.Roles.Student.ToString(),
                    NormalizedName = Helper.Roles.Student.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = instructorRoleId,
                    ConcurrencyStamp = instructorRoleId,
                    Name = Helper.Roles.Instructor.ToString(),
                    NormalizedName = Helper.Roles.Instructor.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = Helper.Roles.Admin.ToString(),
                    NormalizedName = Helper.Roles.Admin.ToString().ToUpper()
                },
                //new
                new IdentityRole
                {
                    Id = parentRoleId,
                    ConcurrencyStamp = parentRoleId,
                    Name = Helper.Roles.Parent.ToString(),
                    NormalizedName = Helper.Roles.Parent.ToString().ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Seeding Admin into the database
            var ADMIN_ID = "5873cd1d-8878-41c8-9b4c-130989a7355c";
            var EMAIL = "admin@LearnHub.com";
            var PASSWORD = "Admin@123";
            var adminUser = new IdentityUser
            { 
                Id = ADMIN_ID,
                ConcurrencyStamp = ADMIN_ID,
                Email = EMAIL,
                UserName = EMAIL,
                NormalizedEmail = EMAIL.ToUpper(),
                EmailConfirmed = true,
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, PASSWORD);

            builder.Entity<IdentityUser>().HasData(adminUser);
            builder.Entity<IdentityUserRole<String>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = ADMIN_ID
            });

            // List of difficulty for Seeding Difficulties into the database
            var difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                    Id = (int) DifficultyLevel.Hard,
                    Name = DifficultyLevel.Hard.ToString()
                },

                new Difficulty
                {
                    Id = (int) DifficultyLevel.Medium,
                    Name = DifficultyLevel.Medium.ToString(),
                },

                new Difficulty
                {
                    Id = (int) DifficultyLevel.Easy,
                    Name = DifficultyLevel.Easy.ToString()
                }
            };
            // List of Semesters for Seeding semesters into the database
            var semesters = new List<Semester>
            {
                new Semester
                {
                    Id = (int) SemesterNumber.First,
                    SemesterName = SemesterNumber.First.ToString()
                },
                new Semester
                {
                    Id = (int) SemesterNumber.Second,
                    SemesterName = SemesterNumber.Second.ToString()
                },
                new Semester
                {
                    Id = (int) SemesterNumber.Summer,
                    SemesterName = SemesterNumber.Summer.ToString()
                }
            };
            // List of QuestionTypes for Seeding semesters into the database
            var questionTypes = new List<Models.Domain.QuestionType>
            {
                new Models.Domain.QuestionType
                {
                    Id = (int) Helper.QuestionType.MultipleChoice,
                    Name = Helper.QuestionType.MultipleChoice.ToString()
                },

                new Models.Domain.QuestionType
                {
                    Id = (int) Helper.QuestionType.Essay,
                    Name = Helper.QuestionType.Essay.ToString()
                },

                new Models.Domain.QuestionType
                {
                    Id = (int) Helper.QuestionType.TrueOrFalse,
                    Name = Helper.QuestionType.TrueOrFalse.ToString()
                }
            };
            // List of MaterialTypes for Seeding semesters into the database
            var materialTypes = new List<Models.Domain.MaterialType>
            {
                new Models.Domain.MaterialType
                {
                    Id = (int )Helper.MaterialType.RecordedLectures,
                    Name = Helper.MaterialType.RecordedLectures.ToString()
                },

                new Models.Domain.MaterialType
                {
                    Id = (int )Helper.MaterialType.LectureSlides,
                    Name = Helper.MaterialType.LectureSlides.ToString()
                },

                new Models.Domain.MaterialType
                {
                    Id = (int )Helper.MaterialType.RecordedLabs,
                    Name = Helper.MaterialType.RecordedLabs.ToString()
                },

                new Models.Domain.MaterialType
                {
                    Id = (int )Helper.MaterialType.LabSlides,
                    Name = Helper.MaterialType.LabSlides.ToString()
                }
            };


            // Set Configuration of Student
            builder.Entity<Student>(entity =>
            {
                entity.Property(s => s.Id).ValueGeneratedNever();
            });

            // Set Configuration of Semester
            builder.Entity<Semester>(entity =>
            {
                entity.Property(s => s.Id).ValueGeneratedNever();

                entity.HasData(semesters);
            });

            // Set Configuration of Difficulty
            builder.Entity<Difficulty>(entity =>
            {
                entity.Property(d => d.Id).ValueGeneratedNever();

                entity.HasData(difficulties);
            });

            // Set Configuration of QuestionType
            builder.Entity<Models.Domain.QuestionType>(entity =>
            {
                entity.Property(qt => qt.Id).ValueGeneratedNever();

                entity.HasData(questionTypes);
            });

            // Set Configuration of MaterialType
            builder.Entity<Models.Domain.MaterialType>(entity =>
            {
                entity.Property(mt => mt.Id).ValueGeneratedNever();

                entity.HasData(materialTypes);
            });

            // Set Configuration of AssignmentQuestion
            builder.Entity<AssignmentQuestion>(entity =>
            {
                entity.HasOne(aq => aq.Question)
                .WithMany(q => q.AssignmentQuestions)
                .HasForeignKey(aq => aq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}