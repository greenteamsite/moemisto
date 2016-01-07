using System.ComponentModel.DataAnnotations;

namespace Moemisto.Data.Entities
{
    public class ArticleSource
    {
        public int ArticleSourceId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Url { get; set; }
    }
}
