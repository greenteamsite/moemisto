using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;

namespace Moemisto.UI.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsContext _context;
        private const int PageCountArticleBox = 10;
        private const int PageCountNewsBox = 12;
        private const int PageCountListByCategoryBox = 20;
        public NewsController(NewsContext context)
        {
            _context = context;
        }
        [Route("~/news")]
        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            ViewBag.Keywords = "новини, київські новини, статті, аналітика ";
            ViewBag.Description = "Цікаві київські новини: політичні, культурні, про транспорт, спортивні і соціальні. Читати останні київські новини варто тут!";
            var model = new NewsIndexVm
            {
                TopNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetTopNews()),
                Pagination =  new PaginationVm
                {
                    Id = Guid.NewGuid(),
                    IdBox = "#lastNewsBox",
                    CountPages = _context.GetLastNewsCountPages(PageCountNewsBox),
                    CountMaxVisiblePages = 7,
                    Url = Url.Action("LastNewsBox", "News")
                },
                //LastArticles = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastArticle(1, PageCountArticleBox, category)),
                PaginationArticle = new PaginationVm
                {
                    Id = Guid.NewGuid(),
                    IdBox = "#lastArticleBox",
                    CountPages = _context.GetLastArticleCountPages(PageCountArticleBox),
                    CountMaxVisiblePages = 7,
                    Url = Url.Action("LastArticleBox", "News")
                }
            }; 
            return View(model);
        }

        [Route("~/news/cat/{category}")]
        [OutputCache(Duration = 600)]
        public ActionResult ListByCategory(string category)
        {
            ViewBag.Keywords = "новини, київські новини, статті, аналітика ";
            ViewBag.Description = "Цікаві київські новини: політичні, культурні, про транспорт, спортивні і соціальні. Читати останні київські новини варто тут!";
            var model = new NewsListByCategoryVm
            {
                TopNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetTopNews(category)),
                Pagination = new PaginationVm
                {
                    Id = Guid.NewGuid(),
                    IdBox = "#lastNewsBox",
                    CountPages = _context.GetLastArticleCountPagesByCat(PageCountListByCategoryBox, category),
                    CountMaxVisiblePages = 7,
                    Url = Url.Action("LastNewsBoxByCategory", "News")
                },
                CategoryUrl = category,
                CategoryTitle = _context.GetCategoryTitleByUrl(category)
            };

            TempData["categoryUrl"] = category;
            return View(model);
        }

        [OutputCache(Duration = 3600)]
        public ActionResult Details(string url)
        {
            int id;
            if (int.TryParse(url, out id))
            {
                string oldUrl = _context.GetArticleUrlById(id);
                if (!string.IsNullOrEmpty(oldUrl))
                {
                    return RedirectToActionPermanent("Details", new { url = oldUrl });
                }
            }
            var modelDb = _context.GetArticle(url);
            if (modelDb == null)
            {
                return RedirectToAction("Index");
            }
            var model = Mapper.Map<NewsDetailsVm>(modelDb);
            model.SimilarNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetOtherNews(model.ArticleId));
            ViewBag.Keywords = model.Tags;
            ViewBag.Description = model.ShortInfoCutted(150);
            if (Request.Url != null)
            {
                ViewBag.OgImage = string.Format("{0}{1}",
                    Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, string.Empty), model.PictureUrlSmall);
            }
            return View(model);
        }

        public PartialViewResult LastNewsBox(int page)
        {
            var model = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastNews(page, PageCountNewsBox));
            return PartialView("_LastNewsBox", model);
        }
        public PartialViewResult LastArticleBox(int page)
        {
            var model = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastArticle(page, PageCountArticleBox));
            return PartialView("_LastNewsBox", model);
        }

        public ActionResult LastNewsBoxByCategory(int page)
        {
            string categoryUrl = TempData.Peek("categoryUrl").ToString();
            var model = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastArticleByCategory(page, PageCountListByCategoryBox, categoryUrl));
            return PartialView("_LastNewsBox", model);
        }
    }
}