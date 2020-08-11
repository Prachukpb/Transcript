using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transcript.Data;
using Transcript.Models;

namespace Transcript.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly SchoolContext _context;

        public PeopleController(SchoolContext context)
        {
            _context = context;
        }

        //Challenge 1 - Add an endpoint to retrieve a student's transcript given the ID of the student:
        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetPerson(int id)
        {
            // Get the student details from table People for the given id.
            var StudentData = await _context.Person.Where(a => a.PersonId == id & a.Discriminator == "Student").FirstOrDefaultAsync();

            // If the id is invalid or its not a student then return the appropriate status code else proceed.
            if (StudentData == null)
            {
                return NotFound();
            }

            // Get the student's grades and the course details from tables StudentGrade and Course. Create a List based on the condition to avoid null grade values.
            var StudentGrades = _context.StudentGrade.Where(b => b.StudentId == id & b.Grade != null).Include(a => a.Course)
                .Select(item => new grades { courseId = item.CourseId, title = item.Course.Title, credits = item.Course.Credits, grade = item.Grade }).ToList();

            // Calculate the GPA from the data stored in StudentGrades variable.
            // Assign GPATotal to 0.00 initially.
            var GpaTotal = Convert.ToDecimal("0.0");
            if (StudentGrades.Count() > 0)
            {
                GpaTotal = Math.Round(Convert.ToDecimal(StudentGrades.Sum(x => x.grade) / StudentGrades.Count()), 2);
            }

            // Create a new object and assign the values to it. The Student model is designed based on the output json format.
            var Challenge_1 = new Student()
            {
                studentId = StudentData.PersonId,
                firstName = StudentData.FirstName,
                lastName = StudentData.LastName,
                gpa = GpaTotal,
                grades = StudentGrades
            };

            //Return the model with the requested data.
            return Challenge_1;
        }

        //Challenge 2 - Add an endpoint to return a list of students and their GPAs.
        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetPerson()
        {
            //Get the list of all student details from table People.
            var StudentData = await _context.Person.Where(a => a.Discriminator == "Student").ToListAsync();

            //Generate a new Student list object to store the data.
            var Challenge_2 = new List<Student>();

            // Initiate a foreach loop for StudentData.
            foreach (var item in StudentData)
            {
                // Calculate the GPA same like in the previous function. 
                var StudentGrades = _context.StudentGrade.Where(b => b.StudentId == item.PersonId & b.Grade != null).Include(a => a.Course)
                .Select(item1 => new grades { courseId = item1.CourseId, title = item1.Course.Title, credits = item1.Course.Credits, grade = item1.Grade }).ToList();

                // Calculating GPA on the fly is better because if there is any change in the grades table it will not affet the GPA calculations. 
                //This way when we request for GPA we will get the most updated data. 
                var GpaTotal = Convert.ToDecimal("0.0");
                if (StudentGrades.Count() > 0)
                {
                    GpaTotal = Math.Round(Convert.ToDecimal(StudentGrades.Sum(x => x.grade) / StudentGrades.Count()), 2);
                }

                //Create a new object and assign the values to it.
                var StudentDataList = new Student()
                {
                    studentId = item.PersonId,
                    firstName = item.FirstName,
                    lastName = item.LastName,
                    gpa = GpaTotal
                };
                //Add the StudentDataList record to the final list. This keeps adding data to the list untill the loop runs.
                Challenge_2.Add(StudentDataList);
            }

            //Return the list of student details with GPA.
            return Challenge_2;
        }

        //Challenge 3 & 4 - Add an endpoint to insert a student grade:
        // POST: api/People/PostGrades
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<StudentGrade>> PostGrades(StudentGrade StudentGrade)
        {
            // Get all the required data like student, corurse and grade details
            var StudentData = await _context.Person.Where(a => a.PersonId == StudentGrade.StudentId & a.Discriminator == "Student").FirstOrDefaultAsync();
            var CourseData = await _context.Course.Where(a => a.CourseId == StudentGrade.CourseId).FirstOrDefaultAsync();
            var StudentGrades = await _context.StudentGrade.Where(a => a.StudentId == StudentGrade.StudentId & a.CourseId == StudentGrade.CourseId).FirstOrDefaultAsync();

            //Check for 
            //1.studentId must be a valid student ID. 
            //2.courseId must be a valid course ID.
            if (StudentData != null & CourseData != null)
            {
                //3.grade must be null or a numeric value between 0.00 and 4.00 inclusive.(Challenge 3)
                if (StudentGrade.Grade == null || (StudentGrade.Grade >= Convert.ToDecimal(0.00) & StudentGrade.Grade <= Convert.ToDecimal(4.00)))
                {
                    //4.The combination of CourseID and StudentID must be unique.(Challenge 3)
                    if (StudentGrades == null)
                    {
                        _context.StudentGrade.Add(StudentGrade);
                        await _context.SaveChangesAsync();
                        //On success return the data added along with the tabe id.
                        return CreatedAtAction(nameof(GetPerson), new { id = StudentGrade.EnrollmentId }, StudentGrade);
                    }
                    //Return Custom error messages based on the post data.
                    else
                        return BadRequest("StudentId and CourseId combination already present in the datatable");
                }
                else
                    return BadRequest("Grade must be null or a numeric value between 0.00 and 4.00 inclusive.");
            }
            else
                return NotFound("StudentId or courseId is invalid");
        }
    }
}