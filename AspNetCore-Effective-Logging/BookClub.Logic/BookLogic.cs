using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BookClub.Data;
using BookClub.Logic.Models;
using BookClub.Entities;
using System;
using System.Linq;

namespace BookClub.Logic
{
    public class BookLogic : IBookLogic
    {
        private readonly IBookRepository _repo;

        public BookLogic(IBookRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = _repo.GetAllBooks();

            var bookList = new List<BookModel>();
            foreach (var book in books)
            {
                bookList.Add(await GetBookModelFromBook(book));
            }

            return bookList;
        }

        private async Task<BookModel> GetBookModelFromBook(Book book)
        {
            var bookToReturn = new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Category = book.Category,
                Submitter = GetSubmitterFromId(book.Submitter)
            };
            using (var httpClient = new HttpClient())
            {
                var uri = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{book.Isbn}";

                try
                {
                    var response = await httpClient.GetAsync(uri);
                    var content = await response.Content.ReadAsStringAsync();
                    var bookResponse = JsonConvert.DeserializeObject<GoogleBookResponse>(content);

                    var thisBook = bookResponse?.Items?.FirstOrDefault();
                    if (thisBook != null)
                    {
                        bookToReturn.Description = thisBook.VolumeInfo?.Description;
                        bookToReturn.PageCount = thisBook.VolumeInfo?.PageCount ?? 0;
                        bookToReturn.InfoLink = thisBook.VolumeInfo?.InfoLink;
                        bookToReturn.Thumbnail = thisBook.VolumeInfo?.ImageLinks?.Thumbnail;
                    }
                }
                catch (Exception ex)
                {
                    // it's ok if google api call doesn't work                    
                }
                return bookToReturn;
            }
        }

        private string GetSubmitterFromId(int submitter)
        {
            switch (submitter)
            {
                case 11:
                    return "Bob";
                case 111:
                    return "Erik";
                default:
                    return "Alice";
            }
        }
    }
}
