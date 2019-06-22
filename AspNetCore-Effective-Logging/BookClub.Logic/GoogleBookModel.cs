using System;
using System.Collections.Generic;

namespace BookClub.Logic
{
    public class GoogleBookModel
    {
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public Images ImageLinks {get;set;} 
        public string InfoLink { get; set; }
    }

    public class Images
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }

    public class GoogleBookResponse
    {
        public List<GoogleVolume> Items { get; set; }
    }

    public class GoogleVolume
    {
        public GoogleBookModel VolumeInfo { get; set; }
    }
}
