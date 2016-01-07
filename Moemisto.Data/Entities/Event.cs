using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150)]
        public string TranslitUrl { get; set; }
        [Index]
        [MaxLength(150)]
        public string Title { get; set; }

        public int TypeId { get; set; }
        /// <summary>
        /// Вид події
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual EventType Type { get; set; }
        [MaxLength(100)]
        public string PicturePath { get; set; }
        [MaxLength(150)]
        public string PictureFileName { get; set; }
        /// <summary>
        /// Копія картинки маленького розміру
        /// </summary>
        [MaxLength(150)]
        public string PictureFileNameSmall { get; set; }
        [MaxLength(900)]
        public string ShortInfo { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// Чи дозволена публікація
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// Рейтинг
        /// </summary>
        public int Rate { get; set; }
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Розклад Події
        /// </summary>
        public ICollection<EventSchedule> Schedules { get; set; }

        /// <summary>
        /// Коментарі до події
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}
