using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Moemisto.Data.NoEntities;

namespace Moemisto.UI.Areas.Admin.Services.Sitemap
{
    public static class SiteMapHelper
    {
        public static void SaveSiteMap(List<SiteMapItemDb> siteMapsInfo, string fileName)
        {
            var sitemapGenerator = new SitemapGenerator();
            var xmlDoc =
                sitemapGenerator.GenerateSiteMap(
                    siteMapsInfo.AsParallel().Select(s => new SitemapItem(s.Url, s.LastModified, s.ChangeFrequency)));

            using (var compressedFileStream = new FileStream(fileName, FileMode.Create))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                    CompressionMode.Compress))
                {
                    xmlDoc.Save(compressionStream);
                }
            }
        }
    }
}