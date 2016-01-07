using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class EventScheduleItemVm
    {
        public int PlaceId { get; set; }
        public string PlaceTitle { get; set; }
        public string PlaceUrl { get; set; }
        public string PlaceAddress { get; set; }

        /// <summary>
        /// Date / Times
        /// </summary>
        public Dictionary<string, string> StartEventList { get; set; }
    }
}