using System.Collections.Generic;
using System.Xml.Linq;

namespace Moemisto.UI.Areas.Admin.Services.Sitemap
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
