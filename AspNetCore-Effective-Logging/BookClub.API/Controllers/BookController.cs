using System.Collections.Generic;
using System.Linq;
using BookClub.Data;
using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookClub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookRepository bookRepo, ILogger<BookController> logger)
        {
            _bookRepo = bookRepo;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Book> GetBooks()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            _logger.LogInformation("{UserId} is inside get all books API call.  {claims}",
                userId, User.Claims);
            return _bookRepo.GetAllBooks();
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
