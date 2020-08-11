using Newtonsoft.Json;

namespace Transcript.Models
{
    public partial class StudentGrade
    {
        //This Json Property can be used to customize the property name when returning the JSON data.
        [JsonProperty(PropertyName = "gradeId")]
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public decimal? Grade { get; set; }

        //This Json Property completely ignores the data.
        [JsonIgnore]
        public virtual Course Course { get; set; }

        [JsonIgnore]
        public virtual Person Student { get; set; }
    }
}