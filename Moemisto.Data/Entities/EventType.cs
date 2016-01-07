using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class EventType
    {
        public int EventTypeId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string TranslitUrl { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        /// <summary>
        /// Назва типу в однині
        /// </summary>
        [MaxLength(50)]
        public string TitleOne { get; set; }
    }
}
