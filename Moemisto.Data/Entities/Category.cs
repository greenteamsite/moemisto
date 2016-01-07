using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string TranslitUrl { get; set; }
        [Index(IsUnique = false)]
        [MaxLength(50)]
        public string ParrentUrl { get; set; }
        [Required]
        public ArticleType Type { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}
