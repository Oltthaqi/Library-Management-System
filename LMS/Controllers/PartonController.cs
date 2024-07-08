using LibraryManagementSystem.Data;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class PartonController : Controller
    {

        private readonly DataContext _context;

        public PartonController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Patron>> Get()
        {
            return Ok(_context.Patrons.ToList());
        }
        [HttpGet("{id:int}")]
        public ActionResult<Patron> Get(int id)
        {
            var patron = _context.Patrons.Find(id);
            if (patron == null)
            {
                return NotFound();
            }

            return Ok(patron);
        }

        [HttpPost("Add")]
        public ActionResult<List<Patron>> AddBooks(Patron b)
        {
            _context.Patrons.Add(b);
            _context.SaveChanges();
            return Ok(_context.Patrons);
        }

        [HttpPut("Update")]
        public ActionResult<List<Patron>> Update(Patron p)
        {
            var patron = _context.Patrons.Find(p.Id);
            if (patron == null)
            {
                return NotFound();
            }
            patron.Name = p.Name;
            patron.Description = p.Description;
            _context.SaveChanges();

            return Ok(_context.Patrons);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<List<Patron>>> Delete(int id)
        {
            var patron = _context.Patrons.Find(id);
            if (patron == null)
            {
                return NotFound();
            }
            _context.Patrons.Remove(patron);
            _context.SaveChanges();
            return Ok(_context.Patrons);
        }

    }
}
