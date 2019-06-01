using System.Collections.Generic;
using BookClub.Data;
using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookClub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            return _bookRepo.GetAllBooks();
            //return _bookRepo.GetAllBooksThrowError();
        }

        [HttpGet("{id}", Name = "Get")]
        public Book Get(int id)
        {
            return new Book();
        }

        // POST: api/Book
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
