using System.Collections.Generic;
using System.Web.Mvc;

namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminEventScheduleItemVm
    {
        public int EventId { get; set; }
        public int ScheduleId { get; set; }
        public SelectListItem PlaceTypeGroupFirst { get; set; }
        public List<SelectListItem> PlaceTypeGroups { get; set; }
        public SelectListItem PlaceTypeFirst { get; set; }
    }
}