using System.Collections.Generic;
using BookClub.Entities;

namespace BookClub.Data
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        List<Book> GetAllBooksThrowError(); // will throw an exception in the SQL layer
    }
}
