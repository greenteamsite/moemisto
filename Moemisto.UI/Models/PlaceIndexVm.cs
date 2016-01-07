using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moemisto.UI.Models
{
    public class PlaceIndexVm
    {
        [UIHint("PlaceGroupBox")]
        public List<PlaceTypeBaseVm> Groups { get; set; }
    }
}