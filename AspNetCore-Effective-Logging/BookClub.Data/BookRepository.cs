using System.Collections.Generic;
using System.Data;
using System.Linq;
using BookClub.Entities;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BookClub.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IDbConnection db, ILogger<BookRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<Book> GetAllBooks()
        {
            _logger.LogInformation("Inside the repository about to call GetAllBooks.");
            var books = _db.Query<Book>("GetAllBooks", commandType: CommandType.StoredProcedure)
                .ToList();
            return books;
        }

        public void SubmitNewBook(Book bookToSubmit, int submitter)
        {
            _db.Execute("InsertBook", new {
                bookToSubmit.Title,
                bookToSubmit.Author,
                Classification = bookToSubmit.Category,
                bookToSubmit.Genre,
                bookToSubmit.Isbn,
                submitter
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
