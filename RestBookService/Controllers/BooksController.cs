using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestBookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/<BooksController>
        [HttpGet]
        public List<Book> Get()
        {
            return BookList.bookList;
        }

        // GET api/<BooksController>/9780553348989
        [HttpGet("{isbn13}")]
        public IActionResult Get(string isbn13)
        {
            Book book = getBook(isbn13);
            if (book == null)
            {
                return NotFound(new { message = "This ISBN13 does not match any book in the database." });
            }
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public IActionResult Post([FromBody] Book value)
        {
            Book book = getBook(value.Isbn13);
            if (book != null)
            {
                return Conflict(new { message = "This ISBN13 already exists in the database." });
            }
            BookList.bookList.Add(value);
            return CreatedAtAction("Get", new { isbn13 = value.Isbn13 }, value);
        }

        // PUT api/<BooksController>/6780553348989
        [HttpPut("{isbn13}")]
        public IActionResult Put(string isbn13, [FromBody] Book value)
        {
            if (isbn13 != value.Isbn13)
            {
                return BadRequest(new { message = "Invalid given ISBN13." });
            }

            Book book = getBook(isbn13);

            if (book != null)
            {
                book.Author = value.Author;
                book.PageNumber = value.PageNumber;
                book.Title = value.Title;
            }
            else
            {
                return NotFound(new { message = "This ISBN13 does not exist in the database." });
            }
            return NoContent();
        }

        // DELETE api/<BooksController>/6780553348989
        [HttpDelete("{isbn13}")]
        public IActionResult Delete(string isbn13)
        {
            Book book = getBook(isbn13);

            if (book != null)
                BookList.bookList.Remove(book);
            else
            {
                return NotFound(new { message = "No Book Found Corresponding To The Provided ISBN13" });
            }
            return Ok(book);
        }

        private Book getBook(string isbn13)
        {
            Book book = BookList.bookList.FirstOrDefault(b => b.Isbn13 == isbn13);
            return book;
        }
    }
}
