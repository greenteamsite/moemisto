using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moemisto.Data.Contexts.Admin;
using Moemisto.Data.Entities;
using Moemisto.Data.NoEntities;
using Moemisto.UI.Areas.Admin.Services.Sitemap;

namespace Moemisto.UI.Areas.Admin.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        private readonly AdminHomeContext _context;

        public AdminHomeController(AdminHomeContext context)
        {
            _context = context;
        }

        [OutputCache(Duration = 3600)]
        public async Task<ActionResult> Index()
        {
            #region Generate SiteMap

            string mainUrl = "http://moemisto.com.ua/";
            var siteMapPath = Server.MapPath("/");
            var sitemapGenerator = new SitemapGenerator();

            var years = await _context.GetYearsForSiteMap();

            foreach (var year in years)
            {
                var fileName = string.Format("{0}/sitemap{1}.xml.gz", siteMapPath, year);
                if (year != DateTime.Now.Year && System.IO.File.Exists(fileName))
                {
                    continue;
                }
                var siteMapsInfo = await _context.GetSiteMapInfoByYear(year);
                SiteMapHelper.SaveSiteMap(siteMapsInfo, fileName);
            }

            // Places
            var fileNamePlaces = string.Format("{0}/sitemap-places.xml.gz", siteMapPath);
            var siteMapsPlaces = await _context.GetSiteMapInfoPlaces();

            SiteMapHelper.SaveSiteMap(siteMapsPlaces, fileNamePlaces);

            // Site Sections
            var fileNameSections = string.Format("{0}/sitemap-sections.xml.gz", siteMapPath);
            var siteMapsSections = new List<SiteMapItemDb>
            {
                new SiteMapItemDb { Url = mainUrl, ChangeFrequency = SitemapChangeFrequency.Hourly },
                new SiteMapItemDb { Url = string.Format("{0}news", mainUrl), ChangeFrequency = SitemapChangeFrequency.Hourly },
                new SiteMapItemDb { Url = string.Format("{0}afisha", mainUrl), ChangeFrequency = SitemapChangeFrequency.Hourly },
                new SiteMapItemDb { Url = string.Format("{0}travel", mainUrl), ChangeFrequency = SitemapChangeFrequency.Hourly },
                new SiteMapItemDb { Url = string.Format("{0}places", mainUrl), ChangeFrequency = SitemapChangeFrequency.Hourly },
                new SiteMapItemDb { Url = string.Format("{0}about", mainUrl), ChangeFrequency = SitemapChangeFrequency.Monthly }
            };

            SiteMapHelper.SaveSiteMap(siteMapsSections, fileNameSections);

            //var listSiteMapFiles =
            //    years.Select(s => new SitemapItem(string.Format("{0}sitemap{1}.xml.gz", mainUrl, s)))
            //        .ToList();
            //
            //listSiteMapFiles.Add(new SitemapItem(string.Format("{0}sitemap-places.xml.gz", mainUrl)));
            //listSiteMapFiles.Add(new SitemapItem(string.Format("{0}sitemap-sections.xml.gz", mainUrl)));

            //var mainXmlDoc = sitemapGenerator.GenerateSiteMap(listSiteMapFiles);
            //mainXmlDoc.Save(string.Format("{0}/sitemap.xml", siteMapPath));

            #endregion

            return View();
        }
    }
}