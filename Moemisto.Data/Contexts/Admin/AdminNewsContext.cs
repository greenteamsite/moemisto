using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts.Admin
{
    public class AdminNewsContext: BaseContext
    {
        private readonly DbMmContext _context;

        public AdminNewsContext(DbMmContext context) : base(context)
        {
            _context = context;
        }
        public List<Article> GetNewsByDateType(DateTime date, ArticleType articleType)
        {
            return _context.Articles.Include("Pictures").Where(w => w.DatePublish.Year == date.Year && w.DatePublish.Month == date.Month && w.DatePublish.Day == date.Day && w.Category.Type == articleType).ToList();
        }

        public List<ArticleType> GetNewsTypes(DateTime date)
        {
            return _context.Categories.Select(s => s.Type).Distinct().ToList();
            //return
            //    _context.Articles.Where(
            //        w =>
            //            w.DatePublish.Year == date.Year && w.DatePublish.Month == date.Month &&
            //            w.DatePublish.Day == date.Day).Select(s => s.Category.Type).Distinct().ToList();
        }
        public List<Category> GetNewsCategory()
        {
            return
                _context.Articles.Select(s => s.Category).Distinct()
                    .OrderBy(o => o.Type)
                    .ThenBy(t => t.Name).ToList();
        }
        public int CreateEmptyArticle()
        {
            var article = new Article
            {
                DateCreate = DateTime.Now,
                DatePublish = DateTime.Now.AddHours(3),
                CategoryId = GetFirstArticleCategory(),
                TranslitUrl = Guid.NewGuid().ToString()
            };
            _context.Articles.Add(article);
            _context.SaveChanges();
            article.Title = string.Format("Нова стаття #: {0}", article.ArticleId);
            _context.SaveChanges();
            return article.ArticleId;
        }
        /// <summary>
        /// Temporary (Change to News (the most common Article))
        /// </summary>
        /// <returns></returns>
        public int GetFirstArticleCategory()
        {
            return _context.Categories.Select(s => s.CategoryId).First();
        }

        public bool SaveArticle(Article article)
        {
            var articleDb = GetArticle(article.ArticleId);
            if (string.IsNullOrEmpty(article.TranslitUrl))
            {
                string tr =
                    _context.Database.SqlQuery<string>(String.Format("Select dbo.ChangeToTranslit('{0}')",
                        article.Title.Replace("'", "-"))).Single();
                bool urlExist =
                    _context.Articles.Where(w => w.ArticleId != article.ArticleId).Any(a => a.TranslitUrl == tr);
                if (urlExist)
                {
                    tr = string.Format("{0}_{1}", tr,
                        article.ArticleId.ToString().Substring(article.ArticleId.ToString().Length - 4));
                }
                article.TranslitUrl = tr;
            }
            _context.Entry(articleDb).CurrentValues.SetValues(article);
            _context.SaveChanges();
            return true;
        }

        public Article GetArticle(int articleId)
        {
            return _context.Articles.Include(i => i.Pictures).Single(s => s.ArticleId == articleId);
        }

        public bool RemoveArticle(int id)
        {
            try
            {
                _context.Articles.Remove(GetArticle(id));
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Picture> GetPictureList(int articleId, bool withTop)
        {
            return _context.Pictures.Where(w => w.ArticleId == articleId && (!w.Top || withTop)).ToList();
        }
        public Picture GetPicture(int pictureId)
        {
            return _context.Pictures.SingleOrDefault(w => w.PictureId == pictureId);
        }
        public bool AddPicture(int articleId, string title, string path, string url, string urlHomeTop = null, string urlSmall = null, bool top = false)
        {
            if (top)
            {
                SetTopPictureUsual(articleId);
            }
            var picture = new Picture
            {
                Title = title,
                Path = path,
                FileName = url,
                FileNameHomeTop = urlHomeTop,
                FileNameSmall = urlSmall,
                Top = top
            };
            try
            {
                var article = GetArticle(articleId);
                article.Pictures.Add(picture);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemovePicture(int pictureId)
        {
            try
            {
                var picture = _context.Pictures.SingleOrDefault(s => s.PictureId == pictureId);
                if (picture != null)
                {
                    _context.Pictures.Remove(picture);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetTopPictureUsual(int articleId)
        {
            var topPicture =
                _context.Pictures.FirstOrDefault(w => w.ArticleId == articleId && w.Top);
            if (topPicture != null)
            {
                topPicture.Top = false;
                _context.SaveChanges();
            }
        }
    }
}
