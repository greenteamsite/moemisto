using System.ComponentModel.DataAnnotations;
using Moemisto.UI.Helpers;

namespace Moemisto.UI.Models
{
    public class PlaceBaseVm
    {
        public int PlaceId { get; set; }
        public string TranslitUrl { get; set; }
        [MaxLength(150)]
        public int PlaceTypeId { get; set; }

        public string Title { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
        /// <summary>
        /// The nearest Metro
        /// </summary>
        public string Metro { get; set; }
        public string Cost { get; set; }
        /// <summary>
        /// Лінк на місце на карті https://www.google.com.ua/maps
        /// </summary>
        [MaxLength(150)]
        public string MapsUrl { get; set; }
        [MaxLength(100)]
        public string SiteUrl { get; set; }
        /// <summary>
        /// Лінк постера(картинки) події
        /// </summary>
        [MaxLength(250)]
        public string PosterUrl { get; set; }
        /// <summary>
        /// Лінк постера(маленької картинки) події 
        /// </summary>
        [MaxLength(250)]
        public string PosterSmallUrl { get; set; }
        public string Info { get; set; }
        public string Phone { get; set; }
        public string ShortInfoCutted(int length)
        {
            return HelperForVm.TextCut(Info, length);
        }
        /// <summary>
        /// MicroData tag - ItemType
        /// </summary>
        public string MicroDataItemType
        {
            get
            {
                string res = "http://schema.org/Place";
                switch (PlaceTypeId)
                {
                    case 5:
                        res = "Museum";
                        break;
                }
                return res;
            }
        }
    }
}