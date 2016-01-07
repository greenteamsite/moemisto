using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moemisto.Data.Entities;
using Moemisto.Data.NoEntities;

namespace Moemisto.Data.Contexts
{
    public class EventContext: BaseContext
    {
        private readonly DbMmContext _context;

        public EventContext(DbMmContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Event> GetEvents()
        {
            return _context.Events.ToList();
        }
        public List<Event> GetEventsTop10()
        {
            return _context.Events.Take(10).ToList();
        }
        public List<PlaceType> GetPlaceTypesExist()
        {
            return _context.PlaceTypes.ToList();
        }

        public List<EventType> GetEventTypesExist(DateTime startDate)
        {
            return _context.Events.Where(w => w.Schedules.Any(a => a.StartEventList.Any(sa => sa.StartEvent.Year == startDate.Year && sa.StartEvent.Month == startDate.Month &&sa.StartEvent.Day == startDate.Day))).Select(s => s.Type).Distinct().ToList(); 
        }

        public List<EventType> GetEventTypes()
        {
            return _context.EventTypes.ToList();
        }

        public List<Place> GetEventPlaces()
        {
            return _context.Events.SelectMany(s => s.Schedules.Select(p => p.Place)).Distinct().OrderBy(o => o.Title).ToList();
        }

        public Event GetEvent(int id)
        {
            return _context.Events.Include(i => i.Schedules).SingleOrDefault(s => s.EventId == id);
        }
        public Event GetEvent(string url)
        {
            return _context.Events.Include(i => i.Schedules).SingleOrDefault(s => s.TranslitUrl == url);
        }
        public List<Event> GetOtherEvent(int eventId)
        {
            var dateEvent = DateTime.Today; //_context.Events.Where(w => w.EventId == eventId).Select(sel => sel.StartEvent).Single();

            return _context.Events.Where(w => w.EventId != eventId && (w.Schedules.Any(m => m.StartEventList.Any(a => a.StartEvent.Year == dateEvent.Year && a.StartEvent.Month == dateEvent.Month && a.StartEvent.Day == dateEvent.Day)))).ToList(); 
        }

        public int GetEventTypeIdByUrl(string url)
        {
            return _context.EventTypes.Where(w => w.TranslitUrl == url).Select(s => s.EventTypeId).SingleOrDefault();
        }

        public string GetEventTypeUrlById(int id)
        {
            return _context.EventTypes.Where(w => w.EventTypeId == id).Select(s => s.TranslitUrl).SingleOrDefault();
        }

        public List<Place> GetEventPlacesByEventId(int eventId)
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return _context.EventSchedules.Where(w => w.EventId == eventId && w.StartEventList.Any(a => a.StartEvent >= today)).Select(s => s.Place).Distinct().OrderBy(o => o.Title).ToList();
        }

        public Dictionary<string, string> GetEventSchedulesDateTimes(int eventId, int placeId)
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var res =
                _context.EventSchedules.Include(i => i.StartEventList)
                    .Where(w => w.EventId == eventId && w.PlaceId == placeId)
                    .ToList()
                    .SelectMany(
                        sm =>
                            sm.StartEventList
                                .Where(ws => ws.StartEvent >= today)
                                .Select(
                                    sg =>
                                        new
                                        {
                                            dayStr =
                                                new DateTime(sg.StartEvent.Year, sg.StartEvent.Month, sg.StartEvent.Day),
                                            timeStr = sg.StartEvent.ToString("HH:mm")
                                        }))
                    .Distinct()
                    .GroupBy(g => g.dayStr)
                    .Select(s => new
                    {
                        dayStr = s.Key,
                        aggregateTimes =
                            s.Aggregate(new {dayStr = DateTime.MinValue, timeStr = string.Empty},
                                (current, datetime) =>
                                    new
                                    {
                                        current.dayStr,
                                        timeStr = string.Format("{0} {1}, ", current.timeStr, datetime.timeStr)
                                    })
                    })
                    .OrderBy(o => o.dayStr)
                    .Select(
                        s =>
                            new GroupDateEvent
                            {
                                EventDay = s.dayStr,
                                GroupLevel = 0,
                                TimeStr = s.aggregateTimes.timeStr.Substring(0, s.aggregateTimes.timeStr.Length - 2).Trim()
                            }).ToList();

            int group = 0;

            for (int i = 0; i < res.Count - 1; i++)
            {
                res[i].GroupLevel = group;
                if (res[i].TimeStr != res[i + 1].TimeStr || (res[i + 1].EventDay - res[i].EventDay).Days > 1)
                {
                    group ++;
                }
            }
            res.Last().GroupLevel = group;
            var resGroupByDates = res.GroupBy(g => new { g.TimeStr, g.GroupLevel })
                .Select(s => new
                {
                    timeStr = s.Key.TimeStr == "00:00" ? "" : s.Key.TimeStr,
                    aggDays = s.Min(m => m.EventDay) == s.Max(m => m.EventDay)
                        ? s.Min(m => m.EventDay).ToString("dd MMMM")
                        : string.Format("{0} - {1}", s.Min(m => m.EventDay).ToString("dd MMMM"),
                            s.Max(m => m.EventDay).ToString("dd MMMM"))
                });

            return resGroupByDates.ToDictionary(k => k.aggDays, v => v.timeStr);
        }

        public DateTime GetEventStartDate(int eventId)
        {
            DateTime res;
            var preRes = _context.EventSchedules.Where(w => w.EventId == eventId)
                .SelectMany(s => s.StartEventList.Where(we => we.StartEvent >= DateTime.Now).Select(s2 => s2.StartEvent));
            if (preRes.Any())
            {
                res = preRes.Min();
            }
            else
            {
                res = _context.EventSchedules.Where(w => w.EventId == eventId)
                    .SelectMany(s => s.StartEventList.Select(m => m.StartEvent))
                    .GroupBy(g => g)
                    .Select(s => s.Max())
                    .FirstOrDefault();
            }
            return res;
        }
    }
}
