using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : Controller
    {
        private readonly StudentDbContext _studentDbContext;

        public StudentController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await (from Student in _studentDbContext.Students select Student).ToListAsync();
            return Ok(students);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await (from Student in _studentDbContext.Students where Student.Id == id select Student).SingleAsync();

            if (student != null)
            {
                return Ok(student);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (student == null) return BadRequest();

            await _studentDbContext.Students.AddAsync(student);
            await _studentDbContext.SaveChangesAsync();

            return Ok(student);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            var existingStudent = await (from Student in _studentDbContext.Students where Student.Id == id select Student).SingleAsync();

            if (existingStudent != null)
            {
                existingStudent.StudentName = student.StudentName;
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingStudent);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var existingStudent = await (from Student in _studentDbContext.Students where Student.Id == id select Student).SingleAsync();

            if (existingStudent != null)
            {
                _studentDbContext.Students.Remove(existingStudent);
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingStudent);
            }

            return NotFound();
        }
    }
}
