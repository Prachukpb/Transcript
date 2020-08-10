using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Transcript.Models
{
    public partial class StudentGrade
    {
        [JsonProperty(PropertyName = "gradeId")]
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public decimal? Grade { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }
        [JsonIgnore]
        public virtual Person Student { get; set; }
    }
}
