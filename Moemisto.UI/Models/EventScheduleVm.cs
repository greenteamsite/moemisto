namespace Moemisto.UI.Models
{
    public class EventScheduleVm
    {
        public int EventScheduleId { get; set; }

        public string EventTitle { get; set; }
        /// <summary>
        /// Список дат та часу проведення івента в заданому місці Place
        /// </summary>
        public string StartEventListStr { get; set; }
        public int PlaceId { get; set; }
        public PlaceBaseVm Place { get; set; }
        /// <summary>
        /// Ціна з (грн.)
        /// </summary>
        public int PriceFrom { get; set; }
        /// <summary>
        /// Ціна по (грн.)
        /// </summary>
        public int PriceTo { get; set; }
    }
}