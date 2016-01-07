using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Moemisto.Data.Entities;
using Moemisto.Data.NoEntities;

namespace Moemisto.Data.Contexts.Admin
{
    public class AdminHomeContext
    {
        private readonly DbMmContext _context;

        public AdminHomeContext(DbMmContext context)
        {
            _context = context;
        }

        public async Task<List<SiteMapItemDb>> GetSiteMapInfoByYear(int year)
        {
            string mainUrl = "http://moemisto.com.ua/";
            var articles =
             await _context.Articles
                .Where(w => w.DatePublish.Year == year)
                .Select(s => new SiteMapItemDb
                {
                    Url = s.Category.Type != ArticleType.Travel ? mainUrl + s.TranslitUrl : mainUrl + "travel/" + s.TranslitUrl,
                    LastModified = s.DateCreate,
                    ChangeFrequency = SitemapChangeFrequency.Daily
                }).ToListAsync();
            
            var events =
             await _context.Events
                .Where(w => w.DateCreate.Year == year)
                .Select(s => new SiteMapItemDb
                {
                    Url = mainUrl + "afisha/" + s.TranslitUrl,
                    LastModified = s.DateCreate,
                    ChangeFrequency = SitemapChangeFrequency.Daily
                }).ToListAsync();

            articles.AddRange(events);

            return articles;
        }

        public async Task<List<SiteMapItemDb>> GetSiteMapInfoPlaces()
        {
            string mainUrl = "http://moemisto.com.ua/";
            var places =
             await _context.Places
                .Select(s => new SiteMapItemDb
                {
                    Url = mainUrl + "place/" + s.TranslitUrl,
                    ChangeFrequency = SitemapChangeFrequency.Monthly
                }).ToListAsync();

            return places;
        }

        public async Task<List<int>> GetYearsForSiteMap()
        {
            return await 
                _context.Articles.GroupBy(g => SqlFunctions.DatePart("Year", g.DatePublish))
                    .Select(s => s.Key.Value)
                    .ToListAsync();
        }
    }
}
