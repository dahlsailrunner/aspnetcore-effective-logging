using BookClub.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookClub.Logic
{
    public interface IBookLogic
    {
        Task<List<BookModel>> GetAllBooks();
    }
}
