using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Moemisto.UI.Helpers;

namespace Moemisto.UI.Models
{
    public class EventBaseVm
    {
        public int EventId { get; set; }
        public string TranslitUrl { get; set; }
        public int TypeId { get; set; }
        public int PlaceId { get; set; }
        [StringLength(150, ErrorMessage = "Тема матеріалу повинна бути не більше 150 символів.")]
        [Required(ErrorMessage = "Введіть тему матеріалу.")]
        public string Title { get; set; }

        /// <summary>
        /// Лінк постера(картинки) події
        /// </summary>
        [MaxLength(250)]
        public string PosterUrl { get; set; }

        /// <summary>
        /// Лінк постера(маленької картинки) події 
        /// </summary>
        public string PosterSmallUrl { get; set; }
        [StringLength(170, ErrorMessage = "Короткий текст повинен бути не більше 170 символів.")]
        [Required(ErrorMessage = "Введіть короткий текст.")]
        public string ShortInfo { get; set; }
        public string ShortInfoCutted(int length)
        {
            if (string.IsNullOrEmpty(ShortInfo))
            {
                return HelperForVm.TextCut(Info, length);
            }
            return HelperForVm.TextCut(ShortInfo, length);
        }

        [AllowHtml]
        [DataType(DataType.Html)]
        [Required]
        public string Info { get; set; }

        /// <summary>
        /// Чи дозволена публікація
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rate { get; set; }

        public string CategoryName { get; set; }

        public string CategoryUrlName { get; set; }
        public string CategoryUrl { get; set; }

        public string PlaceName { get; set; }
        public string PlaceUrl { get; set; }

        /// <summary>
        /// MicroData tag - ItemType
        /// </summary>
        public string MicroDataItemType {
            get
            {
                string res = "http://schema.org/Event";
                switch (TypeId)
                {
                    case 1:
                        res = "ScreeningEvent";
                        break;
                    case 2:
                        res = "TheaterEvent";
                        break;
                    case 3:
                        res = "MusicEvent";
                        break;
                    case 6:
                        res = "VisualArtsEvent";
                        break;
                    case 7:
                        res = "SaleEvent";
                        break;
                    case 8:
                        res = "Festival";
                        break;
                }
                return res;
            }
        }

    }
}