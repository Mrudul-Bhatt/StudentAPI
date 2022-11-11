using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;

namespace StudentAPI.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        public DbSet<Student>? Students { get; set; }
        public DbSet<Subject>? Subjects { get; set; }
        public DbSet<Mark>? Marks { get; set; }

    }
}
