using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class PlaceListVm
    {
        public List<PlaceBaseVm> PlaceList { get; set; }
        public string GroupTitle { get; set; }
        public int PlaceTypeId { get; set; }
        public int PlaceTypeGroupId { get; set; }
        public string GroupUrl { get; set; }
        public string TypeTitle { get; set; }
        public string TypeTitleOne { get; set; }
    }
}