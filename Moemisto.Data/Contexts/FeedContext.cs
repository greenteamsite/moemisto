using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using Moemisto.Data.Entities;
using Moemisto.Data.NoEntities;

namespace Moemisto.Data.Contexts
{
    public class FeedContext
    {
        private readonly DbMmContext _context;

        public FeedContext(DbMmContext context)
        {
            _context = context;
        }
        public List<FeedItem> GetArticlesToday()
        {
            DateTime today = DateTime.Now;
            return _context.Articles
                .Include(i => i.Category)
                .Where(w => SqlFunctions.DateDiff("DAY", w.DatePublish, today) < 5 && w.DatePublish <= today).Include(i => i.Pictures)
                .OrderByDescending(o => o.DatePublish)
                .Select(s => new FeedItem
                {
                    Id = s.ArticleId, 
                    Title = s.Title,
                    Url = s.Category.Type != ArticleType.Travel ? s.TranslitUrl : "travel/" + s.TranslitUrl, 
                    PictureUrl = s.Pictures.Where(w => w.Top).Select(i => i.Path + i.FileName).FirstOrDefault(),
                    PictureUrlSmall = s.Pictures.Where(w => w.Top).Select(i => i.Path + i.FileNameSmall).FirstOrDefault(),
                    DatePublish = s.DatePublish, 
                    Summary = s.ShortInfo, 
                    Content = s.Info, 
                    CategoryName = s.Category.Name
                })
                .ToList();
        }
    }
}
