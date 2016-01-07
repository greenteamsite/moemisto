using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class HomeContext: BaseContext
    {
        private readonly DbMmContext _context;

        public HomeContext(DbMmContext context) : base(context)
        {
            _context = context;
        }
        public List<Article> GetTopNews()
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now && w.Top).OrderByDescending(o => o.DatePublish).Take(3).ToList();
        }
        public List<Article> GetLastNews()
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now && w.Category.Type != ArticleType.Travel).OrderByDescending(o => o.DatePublish).Take(12).ToList();
        }
        public List<Article> GetLastNewsAfterId(int id)
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.ArticleId < id && w.DatePublish <= DateTime.Now && w.Category.Type != ArticleType.Travel).OrderByDescending(o => o.DatePublish).Take(12).ToList();
        }
        public List<Event> GetInterestedEvents()
        {
            return _context.Events.Include(i => i.Schedules).Where(w => w.Schedules.Any(a => a.StartEventList.Any(sa => sa.StartEvent > DateTime.Now))).OrderBy(x => Guid.NewGuid()).Take(8).ToList();
        }
        public List<Article> GetTopTravels()
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now && w.Category.Type == ArticleType.Travel).OrderByDescending(o => o.DatePublish).Take(4).ToList();
        }

        public Dictionary<string, string> GetBottomMenuNews()
        {
            return
                _context.Articles.Include(i => i.Category).Where(w => w.Category.Type != ArticleType.Travel && w.DatePublish <= DateTime.Now)
                    .Select(s => new { s.Category.TranslitUrl, s.Category.Name })
                    .GroupBy(g => new { g.TranslitUrl, g.Name })
                    .ToDictionary(k => string.Format("/news/cat/{0}", k.Key.TranslitUrl), v => v.Key.Name);
        }

        public Dictionary<string, string> GetBottomMenuEvents()
        {
            return
                _context.Events.Include(i => i.Type)
                    .Where(
                        w =>
                            w.Schedules.Any(
                                a =>
                                    a.StartEventList.Any(
                                        sa =>
                                            sa.StartEvent.Year == DateTime.Now.Year &&
                                            sa.StartEvent.Month == DateTime.Now.Month &&
                                            sa.StartEvent.Day == DateTime.Now.Day)))
                    .OrderBy(o => o.Type.EventTypeId)
                    .Select(s => new {s.Type.TranslitUrl, s.Type.Title})
                    .GroupBy(g => new {g.TranslitUrl, g.Title})
                    .ToDictionary(k => string.Format("/afisha/type/{0}", k.Key.TranslitUrl), v => v.Key.Title);
        }

        public Dictionary<string, string> GetBottomMenuTravels()
        {
            return
                _context.Categories.Where(w => w.Type == ArticleType.Travel)
                    .Select(s => new { s.TranslitUrl, s.Name })
                    .Distinct()
                    .ToDictionary(k => string.Format("/travel/cat/{0}", k.TranslitUrl), v => v.Name);
        }

        public Dictionary<string, string> GetBottomMenuPlaces()
        {
            return _context.PlaceTypeGroups.Select(s => new {s.TranslitUrl, s.Title})
                .ToDictionary(k => string.Format("/places/{0}", k.TranslitUrl), v => v.Title);
        }

    }
}
