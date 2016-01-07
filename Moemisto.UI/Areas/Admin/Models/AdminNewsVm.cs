using System.Collections.Generic;
using System.Web.Mvc;

namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminNewsVm
    {
        public List<SelectListItem> NewsTypes { get; set; }
    }
}