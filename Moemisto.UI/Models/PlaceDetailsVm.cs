using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moemisto.UI.Models
{
    public class PlaceDetailsVm : PlaceBaseVm
    {
        [UIHint("EventsMiddleBox")]
        public List<EventBaseVm> PlaceEvents { get; set; }
        [UIHint("OtherPlaces")]
        public List<PlaceBaseVm> OtherPlaces { get; set; }

        public string TypeTitle { get; set; }
        public string TypeTitleOne { get; set; }
        public int PlaceTypeGroupId { get; set; }
        public string GroupTitle { get; set; }
        public string GroupUrl { get; set; }
        public string TypeUrl { get; set; }
    }
}