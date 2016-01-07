using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts
{
    public class BaseContext
    {
        private readonly DbMmContext _context;

        public BaseContext(DbMmContext context)
        {
            _context = context;
        }
        public List<Article> GetArticles()
        {
            return _context.Articles.Include(i => i.Pictures).Where(w => w.DatePublish <= DateTime.Now).ToList();
        }
        public List<Article> GetLastArticlesByType(int page, int pageCount, ArticleType type, string category = null)
        {
            return _context.Articles
                .Include(i => i.Pictures)
                .Where(w => w.DatePublish <= DateTime.Now && w.Category.Type == type && (category == null || w.Category.TranslitUrl == category))
                .OrderByDescending(o => o.DatePublish).Skip((page - 1) * pageCount)
                .Take(pageCount)
                .ToList();
        }

        public int GetCountLastArticlesByType(ArticleType type, string category)
        {
            return _context.Articles.Count(w => w.DatePublish <= DateTime.Now && w.Category.Type == type && (category == null || w.Category.TranslitUrl == category));
        }
        public List<Event> GetPlaceEvent(int placeId)
        {
            return _context.Events.Where(w => w.Schedules.Any(s => s.PlaceId == placeId && s.StartEventList.Any(a => a.StartEvent > DateTime.Now))).ToList();
        }

        public List<Event> GetEventsByFiltersStoredPr(DateTime eventDate, string eventTypeUrl)
        {
            // Make sure Code First has built the model before we open the connection
            _context.Database.Initialize(force: false);

            // Create a SQL command to execute the sproc
            var cmd = _context.Database.Connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[GetEventsByFilters]";
            SqlParameter parEventDate = new SqlParameter
            {
                ParameterName = "@EventDate",
                SqlDbType = SqlDbType.DateTime,
                IsNullable = true,
                Direction = ParameterDirection.Input,
                Value = eventDate == DateTime.MinValue ? (object)DBNull.Value : eventDate
            };

            var parEventTypeUrl = new SqlParameter
            {
                ParameterName = "@EventTypeUrl",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                IsNullable = true,
                Direction = ParameterDirection.Input,
                Value = string.IsNullOrEmpty(eventTypeUrl) || eventTypeUrl == "all" ? (object)DBNull.Value : eventTypeUrl
            };
            cmd.Parameters.Add(parEventDate);
            cmd.Parameters.Add(parEventTypeUrl);

            try
            {
                // Run the sproc
                _context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();

                // Read Blogs from the first result set
                var events = ((IObjectContextAdapter) _context)
                    .ObjectContext
                    .Translate<Event>(reader, "Events", MergeOption.AppendOnly);
                return events.ToList();
            }
            catch (Exception)
            {
                return new List<Event>();
            }
            finally
            {
                _context.Database.Connection.Close();
            }

        }

        // GetEventsByFiltersStoredPr is used instead it 

        //public List<Event> GetEventsByFilters(DateTime eventDate, string eventTypeUrl)
        //{
        //    DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //    return
        //        _context.Events//.Include(i => i.Schedules)
        //            .Where(
        //                w =>
        //                    (string.IsNullOrEmpty(eventTypeUrl) || w.Type.TranslitUrl == eventTypeUrl)
        //                     &&
        //                    ((eventDate == DateTime.MinValue &&
        //                     (!w.Schedules.Any() ||
        //                      w.Schedules.Any(a => a.StartEventList.Any(sa => sa.StartEvent > today))))
        //                    ||
        //                    (w.Schedules.Any(
        //                        a =>
        //                            a.StartEventList.Any(
        //                                a2 => SqlFunctions.DateDiff("Day", eventDate, a2.StartEvent) == 0)))))
        //            .OrderByDescending(o => o.DateCreate).ToList();
        //}

        public Dictionary<int, Tuple<string, string>> GetPlaceInfo(List<int> eventIds)
        {
            var query = from sch in _context.EventSchedules
                         where eventIds.Contains(sch.EventId)
                         group sch by sch.EventId into g
                         select new
                                {
                                    EventId = g.Key,
                                    Title = g.Min(s => s.Place.Title),
                                    TranslitUrl = g.Min(s => s.Place.TranslitUrl),
                                    TypeTitle = g.Min(s => s.Place.PlaceType.Title),
                                    GroupUrl = g.Min(s => s.Place.PlaceType.PlaceTypeGroup.TranslitUrl),
                                    TypeUrl = g.Min(s => s.Place.PlaceType.TranslitUrl),
                                    CountG = g.Count()
                                };

            var res =
                query.ToDictionary(
                    k => k.EventId, 
                    v => new Tuple<string, string>(
                        v.CountG > 1 ? string.Format("http://moemisto.com.ua/places/{0}/{1}", v.GroupUrl, v.TypeUrl) : String.Format("http://moemisto.com.ua/place/{0}", v.TranslitUrl), 
                        v.CountG > 1 ? v.TypeTitle : v.Title)
                    );

            return res;
        }
    }
}
