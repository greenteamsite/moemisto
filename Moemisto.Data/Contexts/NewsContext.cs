using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class NewsContext: BaseContext
    {
        private readonly DbMmContext _context;

        public NewsContext(DbMmContext context)
            : base(context)
        {
            _context = context;
        }
        public Article GetArticle(int id)
        {
            return _context.Articles.Where(w => w.Category.Type != ArticleType.Travel).Include(i => i.Pictures).Single(s => s.ArticleId == id);
        }

        public Article GetArticle(string url)
        {
            return _context.Articles.Where(w => w.Category.Type != ArticleType.Travel).Include(i => i.Pictures).SingleOrDefault(s => s.TranslitUrl == url);
        }
        public List<Article> GetTopNews(string category = null)
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now && w.Top && w.Category.Type != ArticleType.Travel && (string.IsNullOrEmpty(category) || w.Category.TranslitUrl == category || w.Category.ParrentUrl == category)).OrderByDescending(o => o.DatePublish).Take(4).ToList();
        }
        public List<Article> GetLastNews(int page, int pageCount)
        {
            return GetLastArticlesByType(page, pageCount, ArticleType.News);
        }

        public int GetLastNewsCountPages(int pageCount)
        {
            return (int)Math.Ceiling(GetCountLastArticlesByType(ArticleType.News) / (double)pageCount);
        }

        public List<Article> GetLastArticle(int page, int pageCount)
        {
            return GetLastArticlesByType(page, pageCount, ArticleType.Article);
        }

        public List<Article> GetLastArticleByCategory(int page, int pageCount, string categoryUrl)
        {
            return _context.Articles
                .Include(i => i.Pictures)
                .Where(w => w.DatePublish <= DateTime.Now && (string.IsNullOrEmpty(categoryUrl) || w.Category.TranslitUrl == categoryUrl || w.Category.ParrentUrl == categoryUrl))
                .OrderByDescending(o => o.DatePublish).Skip((page - 1) * pageCount)
                .Take(pageCount)
                .ToList();
        }

        public int GetCountLastArticlesByType(ArticleType type)
        {
            return _context.Articles.Count(w => w.DatePublish <= DateTime.Now && w.Category.Type == type);
        }

        public int GetLastArticleCountPages(int pageCount)
        {
            return (int)Math.Ceiling(GetCountLastArticlesByType(ArticleType.Article) / (double)pageCount);
        }

        public int GetLastArticleCountPagesByCat(int pageCount, string category)
        {
            return (int)Math.Ceiling(GetCountLastArticlesByCategory(category) / (double)pageCount);
        }

        private int GetCountLastArticlesByCategory(string category)
        {
            return _context.Articles.Count(w => w.DatePublish <= DateTime.Now && w.Category.TranslitUrl == category);
        }

        public Tuple<string,string,string> GetBreadCrumb(int categoryId)
        {
            var articleType = _context.Categories.Single(s => s.CategoryId == categoryId).Type;
            if (articleType == ArticleType.Travel)
            {
                return new Tuple<string, string, string>("Статті про подорожі Києвом та з Києва", "Travel", "Index");

            }
            return new Tuple<string, string, string>("Новини і статті про Київ", "News", "Index");
        }
        public List<Article> GetSimilarNews(int id)
        {
            int categoryId = _context.Articles.Include(i => i.Pictures).Where(w => w.ArticleId == id).Select(s => s.CategoryId).Single();
            return _context.Articles.Include("Pictures").Where(w => w.DatePublish <= DateTime.Now && w.ArticleId != id && w.CategoryId == categoryId)
                    .OrderByDescending(o => o.DatePublish).Take(4).ToList();
        }
        public List<Article> GetOtherNews(int id)
        {
            return _context.Articles.Include("Pictures").Where(w => w.DatePublish <= DateTime.Now && w.ArticleId != id)
                    .OrderByDescending(o => o.DatePublish).Take(12).ToList();
        }

        public string GetArticleUrlById(int id)
        {
            return _context.Articles.Where(w => w.ArticleId == id && w.Old).Select(s => s.TranslitUrl).FirstOrDefault();
        }

        public string GetCategoryTitleByUrl(string category)
        {
            return _context.Categories.Where(s => s.TranslitUrl == category).Select(s => s.Name).SingleOrDefault();
        }
    }
}
