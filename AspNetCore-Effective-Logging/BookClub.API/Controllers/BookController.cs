using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClub.Data;
using BookClub.Entities;
using BookClub.Logic;
using BookClub.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookClub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        private readonly IBookLogic _bookLogic;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookRepository bookRepo, IBookLogic bookLogic, ILogger<BookController> logger)
        {
            _bookRepo = bookRepo;
            _bookLogic = bookLogic;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<BookModel>> GetBooks()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            _logger.LogInformation("{UserId} is inside get all books API call.  {claims}",
                userId, User.Claims);

            //using (_logger.BeginScope("Constructing books response for {ScopedUserId}", userId))
            using (_logger.ApiGetAllBooksScope(userId))
            {
                return await _bookLogic.GetAllBooks();
            }            
        }

        [HttpGet("{id}", Name = "Get")]
        public Book Get(int id)
        {
            return new Book();
        }
        
        [HttpPost]
        public void Post([FromBody] Book bookToSubmit)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value);
            _bookRepo.SubmitNewBook(bookToSubmit, userId);
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
