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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 
            builder.Entity<Student>().Property(s => s.Id).ValueGeneratedNever();

            // Seeding roles into the database
            var studentRoleId = "26c2b87c-bd59-48aa-87b6-34b414f8d12e";
            var instructorRoleId = "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d";
            var adminRoleId = "b985b240-2dce-4365-bcf5-c4c792b9076b";
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
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Seeding semesters into the database
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
            builder.Entity<Semester>().HasData(semesters);

            // Seeding Admin into the database
            var ADMIN_ID = "5873cd1d-8878-41c8-9b4c-130989a7355c";
            var EMAIL = "admin@LearnHup.com";
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
        }
    }
}
