using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transcript.Models
{
    public class grades
    {
        public int courseId { get; set; }
        public string title { get; set; }
        public int credits { get; set; }
        public decimal? grade { get; set; }
        
    }

    public class student
    {
        public int studentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public decimal gpa { get; set; }
        //public grades grades { get; set; }
        public virtual ICollection<grades> grades { get; set; }

    }
}
