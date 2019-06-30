using System.ComponentModel.DataAnnotations;

namespace BookClub.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Classification { get; set; }      
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public int Submitter { get; set; }
        public override string ToString()
        {
            return $"{Title}; ISBN: {Isbn}; Author: {Author}";
        }
    }
}
