using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class SearchContext
    {
        private readonly DbMmContext _context;

        public SearchContext(DbMmContext context)
        {
            _context = context;
        }
        public List<Article> SearchArticles(string searchQuery)
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now && w.Title.Contains(searchQuery)).OrderByDescending(o => o.DatePublish).Take(2000).ToList();
        }
        public List<Event> SearchEvents(string searchQuery)
        {
            return _context.Events.Where(w => w.Title.Contains(searchQuery)).OrderByDescending(o => o.Rate).Take(1000).ToList();
        }
        public List<Place> SearchPlaces(string searchQuery)
        {
            return _context.Places.Where(w => w.Title.Contains(searchQuery)).OrderByDescending(o => o.Rate).Take(1000).ToList();
        }

        public int GetSearchResultCountPages(int pageCount, string searchQuery)
        {
            return (int)Math.Ceiling(GetCountAllSearchResult(searchQuery) / (double)pageCount);
        }

        private int GetCountAllSearchResult(string searchQuery)
        {
            return _context.Articles.Count(w => w.DatePublish <= DateTime.Now && w.Title.Contains(searchQuery)) +
                   _context.Events.Count(w => w.Title.Contains(searchQuery)) +
                   _context.Places.Count(w => w.Title.Contains(searchQuery));
        }
    }
}
