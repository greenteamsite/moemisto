using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moemisto.UI.Models
{
    public class PlaceTypesVm
    {
        public List<PlaceTypeBaseVm> Types { get; set; }
        public string GroupTitle { get; set; }
    }
}