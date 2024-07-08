using LibraryManagementSystem.Data;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //private static List<Book> books = new List<Book>(); 

        private readonly DataContext _dataContext;

        public BookController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("getAll")]
        public ActionResult<List<Book>> Get()
        {
            var books = _dataContext.Books.ToList();

            foreach (var book in books)
            {
                if (book.img != null && book.img.Length > 0)
                {
                    // Convert byte array to base64 string
                    book.img64 = Convert.ToBase64String(book.img);
                }
            }

            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = _dataContext.Books.Find(id);

            if (book == null)
            {
                return NotFound("Book was not found");
            }
            return Ok(book);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<List<Book>>> AddBooks([FromForm] Book b, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }


            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] byteArray = memoryStream.ToArray();
                b.img = byteArray;

                b.img64 = Convert.ToBase64String(byteArray);
                _dataContext.Books.Add(b);
                await _dataContext.SaveChangesAsync();

                return Ok(_dataContext.Books.ToList());
            }
        }


        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Book>> Update(int id,  Book b)
        {
            //if (file == null || file.Length == 0)
            //{
            //    return BadRequest("No Image was uploaded");
            //}


            
            var book = await _dataContext.Books.FindAsync(id);
            if (book == null)   
            {
                return NotFound();
            }
            //if (file != null && file.Length > 0)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await file.CopyToAsync(memoryStream);
            //        byte[] bytes = memoryStream.ToArray();
            //        book.img = bytes;
            //        book.img64 = Convert.ToBase64String(bytes);
            //    }
            //}
            book.Author = string.IsNullOrEmpty(b.Author) ? book.Author : b.Author;
            book.Name = string.IsNullOrEmpty(b.Name) ? book.Name : b.Name;
            book.Title = string.IsNullOrEmpty(b.Title) ? book.Title : b.Title;
            book.Description = string.IsNullOrEmpty(b.Description) ? book.Description : b.Description;
            book.Price = double.Equals(b.Price, null) ? book.Price : b.Price;
            book.Pages = int.Equals(b.Pages, null) ? book.Pages : b.Pages;
            book.img = book.img;
            book.img64 = book.img64;
                
            _dataContext.Books.Update(book);
            await _dataContext.SaveChangesAsync();

            return Ok(new { message = "Book successfully updated", book });
        }




        [HttpDelete("Delete/{id}")]
        public ActionResult<List<Book>> Delete(int id)
        {
            var book = _dataContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            _dataContext.Books.Remove(book);
            _dataContext.SaveChanges();     
            return Ok(_dataContext.Books);
        }

    }
}
