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

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPerson()
        {
            return await _context.Person.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<student>> GetPerson(int id)
        {
            var person = await _context.Person.Where(a => a.PersonId == id & a.Discriminator == "Student").FirstOrDefaultAsync();

            if (person == null)
            {
                return NotFound();
            }

            var stgrades = _context.StudentGrade.Where(b => b.StudentId == id & b.Grade != null).Include(a => a.Course)
                .Select(item => new grades { courseId = item.CourseId, title = item.Course.Title, credits = item.Course.Credits, grade = item.Grade }).ToList();

            var gpa1 = Convert.ToDecimal("0.0");
            if (stgrades != null)
            {
                gpa1 = Math.Round(Convert.ToDecimal(stgrades.Sum(x => x.grade) / stgrades.Count()), 1);
            }
            var challenge1 = new student()
            {
                studentId = person.PersonId,
                firstName = person.FirstName,
                lastName = person.LastName,
                gpa = gpa1,
                grades = stgrades
            };

            return challenge1;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}