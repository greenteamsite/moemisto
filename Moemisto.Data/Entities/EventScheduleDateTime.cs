using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moemisto.Data.Entities
{
    public class EventScheduleDateTime
    {
        public int EventScheduleDateTimeId { get; set; }
        public int EventScheduleId { get; set; }
        [ForeignKey("EventScheduleId")]
        public virtual EventSchedule EventSchedule { get; set; }
        public DateTime StartEvent { get; set; }
    }
}
