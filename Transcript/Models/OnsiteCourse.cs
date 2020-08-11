using System;

namespace Transcript.Models
{
    public partial class OnsiteCourse
    {
        public int CourseId { get; set; }
        public string Location { get; set; }
        public string Days { get; set; }
        public TimeSpan Time { get; set; }

        public virtual Course Course { get; set; }
    }
}