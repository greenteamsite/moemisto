using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class PlaceType
    {
        public int PlaceTypeId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150)]
        public string TranslitUrl { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string ImageUrl { get; set; }
        public int PlaceTypeGroupId { get; set; }
        /// <summary>
        /// Назва типу в однині
        /// </summary>
        public string TitleOne { get; set; }

        public int? Order { get; set; }

        [ForeignKey("PlaceTypeGroupId")]
        public virtual PlaceTypeGroup PlaceTypeGroup { get; set; }
    }
}
