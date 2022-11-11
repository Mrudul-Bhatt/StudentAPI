using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/mark")]
    public class MarksController : Controller
    {
        private readonly StudentDbContext _studentDbContext;

        public MarksController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMarks()
        {
            var marks = await (from Mark in _studentDbContext.Marks select Mark).ToListAsync();
            return Ok(marks);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMarkById(int id)
        {
            var mark = await (from Mark in _studentDbContext.Marks where Mark.Id == id select Mark).SingleAsync();

            if (mark != null)
            {
                return Ok(mark);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{studentName}")]
        public async Task<IActionResult> GetMarkByStudentName(string studentName)
        {
            //var query1 = (from Student in _studentDbContext.Students where Student.StudentName == studentName select Student.Id).ToListAsync();
            //var query2 =   _studentDbContext.Students.Where(s => s.StudentName == studentName).Select(id => id.Id);

            //var query3 = _studentDbContext.Marks.Where(m => m.StudentId == _studentDbContext.Students.Where(s => s.StudentName == studentName).Select(id => id.Id));

            //var query = _studentDbContext.

            var studentMarks = await _studentDbContext.Marks
                 .Where(m =>
                     _studentDbContext.Students
                     .Where(s => s.StudentName == studentName)
                     .Select(id => id.Id)
                     .Contains(m.StudentId)
                     ).ToListAsync();

            if (studentMarks != null)
            {
                return Ok(studentMarks);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{subjectName}")]
        public async Task<IActionResult> GetMarkBySubjectName(string subjectName)
        {
            var subjectMarks = await _studentDbContext.Marks.Where(m =>
            _studentDbContext.Subjects
            .Where(s => s.SubjectName == subjectName)
            .Select(id => id.Id)
            .Contains(m.SubjectId)).ToListAsync();

            if (subjectMarks != null)
            {
                return Ok(subjectMarks);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMark(string studentName, string subjectName, int marks)
        {
            if (studentName == null || subjectName == null || marks == null) return BadRequest();

            var studentId = await _studentDbContext.Students.Where(s => s.StudentName == studentName).Select(id => id.Id).FirstAsync();
            var subjectId = await _studentDbContext.Subjects.Where(s => s.SubjectName == subjectName).Select(id => id.Id).FirstAsync();

            var mark = new Mark()
            {
                SubjectId = subjectId,
                StudentId = studentId,
                StudentMarks = marks,
            };

            await _studentDbContext.Marks.AddAsync(mark);
            await _studentDbContext.SaveChangesAsync();

            return Ok(mark);
        }

        //[HttpPut]
        //[Route("{id}")]
        //public async Task<IActionResult> UpdateMark(int id, Student student)
        //{
        //    var existingStudent = await (from Student in _studentDbContext.Students where Student.Id == id select Student).SingleAsync();

        //    if (existingStudent != null)
        //    {
        //        existingStudent.StudentName = student.StudentName;
        //        await _studentDbContext.SaveChangesAsync();
        //        return Ok(existingStudent);
        //    }

        //    return NotFound();
        //}

        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteStudent(int id)
        //{
        //    var existingStudent = await (from Student in _studentDbContext.Students where Student.Id == id select Student).SingleAsync();

        //    if (existingStudent != null)
        //    {
        //        _studentDbContext.Students.Remove(existingStudent);
        //        await _studentDbContext.SaveChangesAsync();
        //        return Ok(existingStudent);
        //    }

        //    return NotFound();
        //}
    }
}
