using System.Collections.Generic;
using System.Web.Mvc;


namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminEventIndexVm
    {
        // public List<EventDetailsVm> EventsTop10 { get; set; }
        public List<SelectListItem> EventTypes { get; set; }
        public List<SelectListItem> EventPlaces { get; set; }
    }
}