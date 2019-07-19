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
        private readonly ILogger _logger;

        public BookRepository(IDbConnection db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger("Database");
        }

        public List<Book> GetAllBooks()
        {
            // most beneficial for some kind of db transaction potentially
            using (_logger.BeginScope("Doing database work"))  
            {
                //_logger.LogInformation("Inside the repository about to call GetAllBooks.");                        
                _logger.RepoGetBooks();

                //_logger.LogDebug(DataEvents.GetMany, "Debugging information for stored proc: {ProcName}", 
                //                 "GetAllBooks");
                _logger.RepoCallGetMany("GetAllBooks");

                var books = _db.Query<Book>("GetAllBooks", commandType: CommandType.StoredProcedure)
                    .ToList();
                return books;
            }
            
        }

        public List<Book> GetAllBooksBAD()
        {
            using (_logger.BeginScope("Doing BAD database work"))
            {                
                //_logger.LogDebug(DataEvents.GetMany, "Debugging information for stored proc: {ProcName}", 
                //                 "GetAllBooks");
                _logger.RepoCallGetMany("GetAllBooksBAD");

                var books = _db.Query<Book>("GetAllBooks_DoesNotExist", commandType: CommandType.StoredProcedure)
                    .ToList();
                return books;
            }

        }

        public void SubmitNewBook(Book bookToSubmit, int submitter)
        {
            _db.Execute("InsertBook", new {
                bookToSubmit.Title,
                bookToSubmit.Author,
                bookToSubmit.Classification,
                bookToSubmit.Genre,
                bookToSubmit.Isbn,
                submitter
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
