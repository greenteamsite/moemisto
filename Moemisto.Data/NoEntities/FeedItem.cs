using System;

namespace Moemisto.Data.NoEntities
{
    public class FeedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime DatePublish { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string PictureUrl { get; set; }
        public string PictureUrlSmall { get; set; }
        public string CategoryName { get; set; }
    }
}
