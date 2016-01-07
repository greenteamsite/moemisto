using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class EventSchedule
    {
        public int EventScheduleId { get; set; }
        /// <summary>
        /// Список дат та часу проведення івента в заданому місці Place
        /// </summary>
        public ICollection<EventScheduleDateTime> StartEventList { get; set; }
        public int PlaceId { get; set; }
        [ForeignKey("PlaceId")]
        public virtual Place Place { get; set; }
        /// <summary>
        /// Ціна з (грн.)
        /// </summary>
        public int PriceFrom { get; set; }
        /// <summary>
        /// Ціна по (грн.)
        /// </summary>
        public int PriceTo { get; set; }

        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }
}
