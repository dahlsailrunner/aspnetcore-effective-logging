using System.Collections.Generic;
using System.Data;
using System.Linq;
using BookClub.Entities;
using Dapper;

namespace BookClub.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;

        public BookRepository(IDbConnection db)
        {
            _db = db;
        }

        public List<Book> GetAllBooks()
        {
            var books = _db.Query<Book>("GetAllBooks", commandType: CommandType.StoredProcedure)
                .ToList();
            return books;
        }
    }
}
