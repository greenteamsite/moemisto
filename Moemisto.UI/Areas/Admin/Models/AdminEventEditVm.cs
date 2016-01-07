using System.Collections.Generic;
using System.Web.Mvc;
using Moemisto.UI.Models;

namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminEventEditVm
    {
        public List<SelectListItem> EventTypes { get; set; }
        /// <summary>
        /// Інфо події
        /// </summary>
        public EventBaseVm Event { get; set; }
        /// <summary>
        /// Розклад Події
        /// </summary>
        public List<EventScheduleVm> Schedules { get; set; }
    }
}