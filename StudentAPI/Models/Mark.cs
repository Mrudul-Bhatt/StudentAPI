using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        public int StudentMarks { get; set; }
    }
}
