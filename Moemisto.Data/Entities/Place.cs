using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class Place
    {
        public int PlaceId { get; set; }
        [Index]
        public int PlaceOldTelId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150)]
        public string TranslitUrl { get; set; }
        public int PlaceTypeId { get; set; }
        [ForeignKey("PlaceTypeId")]
        public virtual PlaceType PlaceType { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
        /// <summary>
        /// The nearest Metro
        /// </summary>
        [MaxLength(100)]
        public string Metro { get; set; }
        /// <summary>
        /// Лінк на місце на карті https://www.google.com.ua/maps
        /// </summary>
        [MaxLength(300)]
        public string MapsUrl { get; set; }
        [MaxLength(100)]
        public string SiteUrl { get; set; }
        [MaxLength(100)]
        public string PicturePath { get; set; }
        [MaxLength(150)]
        public string PictureFileName { get; set; }
        /// <summary>
        /// Копія картинки маленького розміру
        /// </summary>
        [MaxLength(150)]
        public string PictureFileNameSmall { get; set; }
        [MaxLength(150)]
        public string Phone { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// Коментарі до місця
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        public int Rate { get; set; }
    }
}
