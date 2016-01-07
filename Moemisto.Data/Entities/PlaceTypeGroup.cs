using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class PlaceTypeGroup
    {
        public int PlaceTypeGroupId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150)]
        public string TranslitUrl { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string ImageUrl { get; set; }
        public int? Order { get; set; }

    }
}
