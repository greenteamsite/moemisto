using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Moemisto.UI.Helpers;

namespace Moemisto.UI.Models
{
    public class ArticleBaseVm
    {
        public int ArticleId { get; set; }
        public string TranslitUrl { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Тема матеріалу повинна бути не більше 150 символів.")]
        public string Title { get; set; }
        [StringLength(54, ErrorMessage = "Коротка тема повиненна бути не більше 54 символів.")]
        [Required(ErrorMessage = "Введіть коротку тему.")]
        public string TitleShort { get; set; }
        [StringLength(170, ErrorMessage = "Короткий текст повинен бути не більше 170 символів.")]
        [Required(ErrorMessage = "Введіть короткий текст.")]
        public string ShortInfo { get; set; }

        public string ShortInfoCutted(int length)
        {
            return HelperForVm.TextCut(ShortInfo, length);
        }
        
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Info { get; set; }
        public int CategoryId { get; set; }
        [MaxLength(150)]
        public string Tags { get; set; }

        [MaxLength(150)]
        public string PictureUrlTop { get; set; }
        /// <summary>
        /// Картинка середня для відображення на головній в топ-статтях
        /// </summary>
        [MaxLength(150)]
        public string PictureUrlHomeTop { get; set; }
        /// <summary>
        /// Картинка маленька для відображення в різних списках статей
        /// </summary>
        [MaxLength(150)]
        public string PictureUrlSmall { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Дата та час публікації")]
        [DataType(DataType.DateTime)]
        public DateTime DatePublish { get; set; }
        //[Display(Name = "Час публікації")]
        //[DataType(DataType.DateTime)]
        //public DateTime TimePublish { get; set; }
        /// <summary>
        /// Для відображення на першій сторінці
        /// </summary>
        [Display(Name = "Топова публікація")]
        public bool Top { get; set; }
        public int Rate { get; set; }
    }
}