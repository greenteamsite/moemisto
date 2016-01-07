using System.Collections.Generic;
using System.Web.Mvc;
using Moemisto.UI.Models;

namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminNewsEditVm : ArticleBaseVm
    {
        public List<SelectListItem> NewsCategoryTypes { get; set; }

    }
}