using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;

namespace Moemisto.UI.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchContext _context;
        private const int CountPages = 30;

        public SearchController(SearchContext context)
        {
            _context = context;
        }

        //[Route("~/search/{query}")]
        public ActionResult Index(string query)
        {
            int resultCountPages = _context.GetSearchResultCountPages(CountPages, query);
            if (String.IsNullOrEmpty(query) || query.Length < 3 || query.Length > 50 || resultCountPages == 0)
            {
                var modelEmpty = new SearchIndexVm();
                TempData.Remove("searchQuery");
                return View(modelEmpty);
            }
            TempData["searchQuery"] = query;
            var model = new SearchIndexVm
            {
                SearchQuery = query,
                Pagination = new PaginationVm
                {
                    Id = Guid.NewGuid(),
                    IdBox = "#searchResultBox",
                    CountPages = resultCountPages,
                    CountMaxVisiblePages = 7,
                    Url = Url.Action("SearchResults", "Search")
                }
            };
            return View(model);
        }

        public ActionResult SearchResults(int page)
        {
            if (TempData.ContainsKey("searchQuery"))
            {
                string searchQuery = (string) TempData.Peek("searchQuery");
                var model = Mapper.Map<List<SearchItemVm>>(_context.SearchArticles(searchQuery));
                model.AddRange(Mapper.Map<List<SearchItemVm>>(_context.SearchEvents(searchQuery)));
                model.AddRange(Mapper.Map<List<SearchItemVm>>(_context.SearchPlaces(searchQuery)));

                return PartialView("_SearchResults", model.Skip((page - 1)*CountPages).Take(CountPages).ToList());
            }
            return Content(String.Empty);
        }
    }
}