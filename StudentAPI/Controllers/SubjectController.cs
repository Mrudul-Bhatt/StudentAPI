using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController : Controller
    {
        private readonly StudentDbContext _studentDbContext;

        public SubjectController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await (from Subject in _studentDbContext.Subjects select Subject).ToListAsync();
            return Ok(subjects);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await (from Subject in _studentDbContext.Subjects where Subject.Id == id select Subject).SingleAsync();

            if (subject != null)
            {
                return Ok(subject);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            if (subject == null) return BadRequest();

            await _studentDbContext.Subjects.AddAsync(subject);
            await _studentDbContext.SaveChangesAsync();

            return Ok(subject);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, Subject subject)
        {
            var existingSubject = await (from Subject in _studentDbContext.Subjects where Subject.Id == id select Subject).SingleAsync();

            if (existingSubject != null)
            {
                existingSubject.SubjectName = subject.SubjectName;
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingSubject);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var existingSubject = await (from Subject in _studentDbContext.Subjects where Subject.Id == id select Subject).SingleAsync();

            if (existingSubject != null)
            {
                _studentDbContext.Subjects.Remove(existingSubject);
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingSubject);
            }

            return NotFound();
        }
    }
}
