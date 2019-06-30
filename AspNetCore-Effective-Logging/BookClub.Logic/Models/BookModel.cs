using System;

namespace BookClub.Logic.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Classification { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Submitter { get; set; }
        public DateTime PublishedDate { get; set; }       
        public int PageCount { get; set; }
        public string Thumbnail { get; set; }
        public string InfoLink { get; set; }
    }
}
