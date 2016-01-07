using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moemisto.UI.Models
{
    public class EventDetailsVm
    {
        /// <summary>
        /// Інфо події
        /// </summary>
        public EventBaseVm Event { get; set; }
        /// <summary>
        /// Розклад Події
        /// </summary>
        public List<EventScheduleItemVm> Schedules { get; set; }
        
        [UIHint("EventsMiddleBox")]
        public List<EventBaseVm> OtherEvents { get; set; }

        //public string FirstPlaceTitle { get; set; }

        [UIHint("EventsMiddleBox")]
        public List<EventBaseVm> PlaceEvents { get; set; }

        public DateTime StartDate { get; set; }
    }
}