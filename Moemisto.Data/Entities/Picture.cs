using System.ComponentModel.DataAnnotations;

namespace Moemisto.Data.Entities
{
    public class Picture
    {
        public int PictureId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Path { get; set; }
        [MaxLength(150)]
        public string FileName { get; set; }
        /// <summary>
        /// Копія картинки середнього розміру
        /// </summary>
        [MaxLength(150)]
        public string FileNameHomeTop { get; set; }
        /// <summary>
        /// Копія картинки маленького розміру
        /// </summary>
        [MaxLength(150)]
        public string FileNameSmall { get; set; }
        /// <summary>
        /// Для відображення першою в новині
        /// </summary>
        public bool Top { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
