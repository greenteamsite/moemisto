using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class TravelContext: BaseContext
    {
        private readonly DbMmContext _context;

        public TravelContext(DbMmContext context)
            : base(context)
        {
            _context = context;
        }
        public Article GetArticle(int id)
        {
            return _context.Articles.Where(w => w.Category.Type == ArticleType.Travel).Include(i => i.Pictures).Single(s => s.ArticleId == id);
        }
        public Article GetArticle(string url)
        {
            return _context.Articles.Where(w => w.Category.Type == ArticleType.Travel).Include(i => i.Pictures).SingleOrDefault(s => s.TranslitUrl == url);
        }
        public List<Article> GetTopTravels(string category)
        {
            return _context.Articles.Include(i => i.Pictures)
                .Where(w => w.DatePublish <= DateTime.Now && w.Top && w.Category.Type == ArticleType.Travel && (category == null || w.Category.TranslitUrl == category))
                .OrderByDescending(o => o.DatePublish)
                .Take(4)
                .ToList();
        }
        public List<Article> GetLastTravels(int page, int pageCount, string category)
        {
            return GetLastArticlesByType(page, pageCount, ArticleType.Travel, category);
        }

        public List<Article> GetSimilarNews(int id)
        {
            int categoryId = _context.Articles.Include(i => i.Pictures).Where(w => w.ArticleId == id).Select(s => s.CategoryId).Single();
            return _context.Articles.Include("Pictures").Where(w => w.DatePublish <= DateTime.Now && w.ArticleId != id && w.CategoryId == categoryId)
                    .OrderByDescending(o => o.DatePublish).Take(12).ToList();
        }
    }
}
