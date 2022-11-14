using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StudentAPI.Data;
using StudentAPI.Models;
using StudentAPI.Models.DTO;

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
            //var marks = await (from Mark in _studentDbContext.Marks select Mark).ToListAsync();

            var marks = await (from stu in _studentDbContext.Students
                               join mark in _studentDbContext.Marks on stu.Id equals mark.StudentId
                               join sub in _studentDbContext.Subjects on mark.SubjectId equals sub.Id
                               select new
                               {
                                   stu.StudentName,
                                   sub.SubjectName,
                                   mark.StudentMarks
                               }).ToListAsync();

            return Ok(marks);
        }

        [HttpGet]
        [Route("searchById/{id}")]
        public async Task<IActionResult> GetMarkById(int id)
        {
            var markById = await (from stu in _studentDbContext.Students
                                  join mark in _studentDbContext.Marks on stu.Id equals mark.StudentId
                                  join sub in _studentDbContext.Subjects on mark.SubjectId equals sub.Id
                                  where mark.Id == id
                                  select new
                                  {
                                      stu.StudentName,
                                      sub.SubjectName,
                                      mark.StudentMarks
                                  }).ToListAsync();

            if (markById != null)
            {
                return Ok(markById);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("searchByStu/{studentName}")]
        public async Task<IActionResult> GetMarkByStudentName(string studentName)
        {
            //var query1 = (from Student in _studentDbContext.Students where Student.StudentName == studentName select Student.Id).ToListAsync();
            //var query2 =   _studentDbContext.Students.Where(s => s.StudentName == studentName).Select(id => id.Id);

            //var query3 = _studentDbContext.Marks.Where(m => m.StudentId == _studentDbContext.Students.Where(s => s.StudentName == studentName).Select(id => id.Id));

            //var query = _studentDbContext.

            //var studentMarks = await _studentDbContext.Marks
            //     .Where(m =>
            //         _studentDbContext.Students
            //         .Where(s => s.StudentName == studentName)
            //         .Select(id => id.Id)
            //         .Contains(m.StudentId)
            //         ).ToListAsync();

            var studentMarks = await (from stu in _studentDbContext.Students
                                      join mark in _studentDbContext.Marks on stu.Id equals mark.StudentId
                                      join sub in _studentDbContext.Subjects on mark.SubjectId equals sub.Id
                                      where stu.StudentName == studentName
                                      select new
                                      {
                                          stu.StudentName,
                                          sub.SubjectName,
                                          mark.StudentMarks
                                      }).ToListAsync();

            if (studentMarks != null)
            {
                return Ok(studentMarks);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("searchBySub/{subjectName}")]
        public async Task<IActionResult> GetMarkBySubjectName(string subjectName)
        {
            //var subjectMarks = await _studentDbContext.Marks.Where(m =>
            //_studentDbContext.Subjects
            //.Where(s => s.SubjectName == subjectName)
            //.Select(id => id.Id)
            //.Contains(m.SubjectId)).ToListAsync();

            var subjectMarks = await (from stu in _studentDbContext.Students
                                      join mark in _studentDbContext.Marks on stu.Id equals mark.StudentId
                                      join sub in _studentDbContext.Subjects on mark.SubjectId equals sub.Id
                                      where sub.SubjectName == subjectName
                                      select new
                                      {
                                          stu.StudentName,
                                          sub.SubjectName,
                                          mark.StudentMarks
                                      }).ToListAsync();

            if (subjectMarks != null)
            {
                return Ok(subjectMarks);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("subjectToppers")]
        public async Task<IActionResult> GetSubjectToppers()
        {
            //var subjectMarks = await _studentDbContext.Marks.Where(m =>
            //_studentDbContext.Subjects
            //.Where(s => s.SubjectName == subjectName)
            //.Select(id => id.Id)
            //.Contains(m.SubjectId)).ToListAsync();

            //var subjectToppers = await (from mark in _studentDbContext.Marks
            //                            group mark.StudentMarks by mark.SubjectId into newGroup
            //                            select new
            //                            {
            //                                SubjectId = newGroup.Key,
            //                                HighestMarks = newGroup.Max()
            //                            }).ToListAsync();

            var subjectToppers = await (from m in _studentDbContext.Marks
                                        join subq in (from mark in _studentDbContext.Marks
                                                      group mark.StudentMarks by mark.SubjectId into newGroup
                                                      select new
                                                      {
                                                          SubjectId = newGroup.Key,
                                                          HighestMarks = newGroup.Max()
                                                      })
                                        on m.SubjectId equals subq.SubjectId
                                        join stu in _studentDbContext.Students on m.StudentId equals stu.Id
                                        join sub in _studentDbContext.Subjects on subq.SubjectId equals sub.Id
                                        where m.StudentMarks == subq.HighestMarks
                                        select new
                                        {
                                            StudentName = stu.StudentName,
                                            SubjectName = sub.SubjectName,
                                            HighestMarks = subq.HighestMarks
                                        }).ToListAsync();

            if (subjectToppers != null)
            {
                return Ok(subjectToppers);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("favSubjectByMarks")]
        public async Task<IActionResult> GetStudentFavSubjectByMarks()
        {
            //var subjectMarks = await _studentDbContext.Marks.Where(m =>
            //_studentDbContext.Subjects
            //.Where(s => s.SubjectName == subjectName)
            //.Select(id => id.Id)
            //.Contains(m.SubjectId)).ToListAsync();

            //var subjectToppers = await (from mark in _studentDbContext.Marks
            //                            group mark.StudentMarks by mark.SubjectId into newGroup
            //                            select new
            //                            {
            //                                SubjectId = newGroup.Key,
            //                                HighestMarks = newGroup.Max()
            //                            }).ToListAsync();

            var favSubjectByMarks = await (from m in _studentDbContext.Marks
                                           join subq in (from mark in _studentDbContext.Marks
                                                         group mark.StudentMarks by mark.StudentId into newGroup
                                                         select new
                                                         {
                                                             StudentId = newGroup.Key,
                                                             HighestMarks = newGroup.Max()
                                                         })
                                           on m.StudentId equals subq.StudentId
                                           join stu in _studentDbContext.Students on subq.StudentId equals stu.Id
                                           join sub in _studentDbContext.Subjects on m.SubjectId equals sub.Id
                                           where m.StudentMarks == subq.HighestMarks
                                           select new
                                           {
                                               StudentName = stu.StudentName,
                                               SubjectName = sub.SubjectName,
                                               HighestMarks = subq.HighestMarks
                                           }).ToListAsync();

            if (favSubjectByMarks != null)
            {
                return Ok(favSubjectByMarks);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMark(MarkDto markDto)
        {
            if (markDto.StudentName == null || markDto.SubjectName == null) return BadRequest();

            if (markDto.StudentMarks < 0 || markDto.StudentMarks > 100) return BadRequest();


            var studentId = await _studentDbContext.Students.Where(s => s.StudentName == markDto.StudentName)
                                                            .Select(id => id.Id).FirstOrDefaultAsync();
            var subjectId = await _studentDbContext.Subjects.Where(s => s.SubjectName == markDto.SubjectName)
                                                            .Select(id => id.Id).FirstOrDefaultAsync();

            if (studentId == 0 || subjectId == 0) return NotFound();

            var isExistingRecord = await (from m in _studentDbContext.Marks
                                          where (m.StudentId == studentId && m.SubjectId == subjectId)
                                          select m).FirstOrDefaultAsync();

            if (isExistingRecord != null) return BadRequest();

            var mark = new Mark()
            {
                SubjectId = subjectId,
                StudentId = studentId,
                StudentMarks = markDto.StudentMarks,
            };

            await _studentDbContext.Marks.AddAsync(mark);
            await _studentDbContext.SaveChangesAsync();

            return Ok(mark);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMark(int id, int updatedMark)
        {
            var existingRecord = await (from mark in _studentDbContext.Marks
                                        where mark.Id == id
                                        select mark).FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                existingRecord.StudentMarks = updatedMark;
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingRecord);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMark(int id)
        {
            var existingRecord = await (from mark in _studentDbContext.Marks
                                        where mark.Id == id
                                        select mark).FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                _studentDbContext.Marks.Remove(existingRecord);
                await _studentDbContext.SaveChangesAsync();
                return Ok(existingRecord);
            }

            return NotFound();
        }
    }
}
