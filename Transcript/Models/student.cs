using Newtonsoft.Json;
using System.Collections.Generic;

namespace Transcript.Models
{
    public class grades
    {
        public int courseId { get; set; }
        public string title { get; set; }
        public int credits { get; set; }
        public decimal? grade { get; set; }
    }

    public class Student
    {
        public int studentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public decimal gpa { get; set; }

        //This Json Property ignores the grades if it is null when returning the JSON data.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<grades> grades { get; set; }
    }
}