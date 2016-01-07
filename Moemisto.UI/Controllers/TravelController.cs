using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;

namespace Moemisto.UI.Controllers
{
    public class TravelController : Controller
    {
        private readonly TravelContext _context;
        public TravelController(TravelContext context)
        {
            _context = context;
        }
        [Route("~/travel/cat/{category}")]
        [Route("~/travel")]
        [OutputCache(Duration = 600)]
        public ActionResult Index(string category)
        {
            ViewBag.Keywords = "подорожі, київські прогулянки, туризм";
            ViewBag.Description = "Статті та новини про подорожі з Києва та по Києву. Тури вихідного дня з Києва та вся корисна інформація для туристів.";
            var model = new TravelIndexVm
            {
                TopTravels = Mapper.Map<List<ArticleBaseVm>>(_context.GetTopTravels(category)),
                LastTravels = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastTravels(1, 20, category))
            };
            return View(model);
        }

        [Route("~/travel/{url}")]
        [OutputCache(Duration = 3600)]
        public ActionResult Details(string url)
        {
            var modelDb = _context.GetArticle(url);
            if (modelDb == null)
            {
                return RedirectToAction("Index");
            }
            var model = Mapper.Map<NewsDetailsVm>(modelDb);
            model.SimilarNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetSimilarNews(model.ArticleId));
            ViewBag.Keywords = model.Tags;
            if (Request.Url != null)
            {
                ViewBag.OgImage = string.Format("{0}{1}",
                    Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, string.Empty), model.PictureUrlSmall);
            }
            ViewBag.Description = model.ShortInfoCutted(150);
            return View(model);
        }

    }
}