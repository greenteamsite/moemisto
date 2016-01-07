using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moemisto.Data.Entities;

namespace Moemisto.Data.Contexts.Admin
{
    public class AdminEventContext: EventContext
    {
        private readonly DbMmContext _context;

        public AdminEventContext(DbMmContext context)
            : base(context)
        {
            _context = context;
        }
        public List<EventSchedule> GetEventSchedules(int eventId)
        {
            return _context.EventSchedules.Include(i => i.StartEventList).Where(w => w.EventId == eventId && w.StartEventList.Any(a => a.StartEvent > DateTime.Now)).OrderBy(o => o.Place.Title).ToList();
        }
        public List<EventType> GetEventTypesAll()
        {
            return _context.EventTypes.Distinct().ToList();
        }

        public Event AddPicture(int eventId, string path, string fileName, string fileNameSmall)
        {
            var eventDb = _context.Events.Single(s => s.EventId == eventId);
            eventDb.PicturePath = path;
            eventDb.PictureFileName = fileName;
            eventDb.PictureFileNameSmall = fileNameSmall;
            _context.SaveChanges();
            return eventDb;
        }

        public bool RemovePicture(int eventId)
        {
            var eventDb = _context.Events.Single(s => s.EventId == eventId);
            eventDb.PicturePath = String.Empty;
            eventDb.PictureFileName = String.Empty;
            eventDb.PictureFileNameSmall = String.Empty;
            _context.SaveChanges();
            return true;
        }

        public bool EventRemove(int eventId)
        {
            var eventDb = _context.Events.Single(s => s.EventId == eventId);
            _context.Events.Remove(eventDb);
            _context.SaveChanges();
            return true;
        }

        public int CreateEmptyEvent()
        {
            var eventNew = new Event
            {
                TypeId = 1,
                DateCreate = DateTime.Now,
                TranslitUrl = Guid.NewGuid().ToString()
            };
            _context.Events.Add(eventNew);
            _context.SaveChanges();
            eventNew.Title = string.Format("Нова подія #: {0}", eventNew.EventId);
            _context.SaveChanges();
            return eventNew.EventId;
        }

        public List<Event> GetEventsByFilters(DateTime eventDate, int eventTypeId)
        {
            string eventTypeUrl = _context.EventTypes.Where(w => w.EventTypeId == eventTypeId).Select(s => s.TranslitUrl).SingleOrDefault();
            var eventsWithSchedule = base.GetEventsByFiltersStoredPr(eventDate, eventTypeUrl);
            var evetsEmpty = _context.Events.Where(w => (eventTypeId == 0 || w.TypeId == eventTypeId) && !w.Schedules.Any()).OrderByDescending(o => o.DateCreate).ToList();
            evetsEmpty.AddRange(eventsWithSchedule);
            return evetsEmpty;
        }

        public bool SaveEvent(Event eventForDb)
        {
            var eventDb = _context.Events.Single(s => s.EventId == eventForDb.EventId);
            string tr = _context.Database.SqlQuery<string>(string.Format("Select dbo.ChangeToTranslit('{0}')", eventForDb.Title.Replace("'", "-"))).Single();
            bool urlExist = _context.Events.Where(w => w.EventId != eventForDb.EventId).Any(a => a.TranslitUrl == tr);
            if (urlExist)
            {
                tr = string.Format("{0}_{1}", tr, eventForDb.EventId.ToString().Substring(eventForDb.EventId.ToString().Length - 4));
            }
            eventForDb.TranslitUrl = tr;
            eventForDb.DateCreate = eventDb.DateCreate;
            eventForDb.PicturePath = eventDb.PicturePath;
            eventForDb.PictureFileName = eventDb.PictureFileName;
            eventForDb.PictureFileNameSmall = eventDb.PictureFileNameSmall;
            _context.Entry(eventDb).CurrentValues.SetValues(eventForDb);
            _context.SaveChanges();
            return true;
        }

        public List<Place> GetEventPlacesByType(int typeid)
        {
            return _context.Places.Where(w => w.PlaceTypeId == typeid).OrderBy(o => o.Title).ToList();
        }

        public string GetEventTitle(int eventid)
        {
            return _context.Events.Where(w => w.EventId == eventid).Select(s => s.Title).SingleOrDefault();
        }

        public List<PlaceType> GetPlaceTypesByGroup(int typegroupid)
        {
            return _context.Places.Include(i => i.PlaceType).Where(w => w.PlaceType.PlaceTypeGroupId == typegroupid).Select(s => s.PlaceType).Distinct().OrderBy(o => o.Order).ToList();
        }

        public List<PlaceTypeGroup> GetPlaceTypeGroups()
        {
            return _context.PlaceTypeGroups.OrderBy(o => o.Order).ToList();
        }

        public int GetPlaceTypeIdFirstByGroup(int typeGroupId)
        {
            return
                _context.Places.Where(w => w.PlaceType.PlaceTypeGroupId == typeGroupId).Select(s => s.PlaceType.PlaceTypeId).Distinct()
                    .FirstOrDefault();
        }

        public EventSchedule GetEventScheduleItem(int eventscheduleid)
        {
            return _context.EventSchedules.Include(i => i.StartEventList).Single(s => s.EventScheduleId == eventscheduleid);
        }

        public bool ScheduleRemove(int scheduleid)
        {
            var schedule = _context.EventSchedules.Single(s => s.EventScheduleId == scheduleid);
            _context.EventSchedules.Remove(schedule);
            _context.SaveChanges();
            return true;
        }

        public int GetPlaceTypeGroupIdByScheduleId(int scheduleId)
        {
            return
                _context.EventSchedules.Where(w => w.EventScheduleId == scheduleId)
                    .Select(s => s.Place.PlaceType.PlaceTypeGroupId)
                    .SingleOrDefault();
        }

        public PlaceType GetPlaceTypeByScheduleId(int scheduleId)
        {
            if (scheduleId == 0)
            {
                return _context.PlaceTypes.OrderBy(o => o.Order).FirstOrDefault();
            }
            return
                _context.EventSchedules.Where(w => w.EventScheduleId == scheduleId)
                    .Select(s => s.Place.PlaceType)
                    .SingleOrDefault();
        }

        public int GetScheduleIdByEventId(int eventId)
        {
            return _context.EventSchedules.Where(w => w.EventId == eventId).Select(s => s.EventScheduleId).FirstOrDefault();
        }

        public int GetPlaceIdByScheduleId(int scheduleId)
        {
            return
                _context.EventSchedules.Where(w => w.EventScheduleId == scheduleId)
                    .Select(s => s.PlaceId)
                    .SingleOrDefault();
        }

        public bool SaveSchedule(EventSchedule schedule)
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var scheduleDb = _context.EventSchedules.SingleOrDefault(s => s.EventScheduleId == schedule.EventScheduleId);
            int eventScheduleId = schedule.EventScheduleId;
            if (scheduleDb != null)
            {
            // for removing
                var scheduleDateTimesDb =
                    _context.EventScheduleDateTimes.Where(w => w.EventScheduleId == eventScheduleId && w.StartEvent >= today);

                foreach (var dateTimeItem in scheduleDateTimesDb)
                {
                    if (schedule.StartEventList.All(a => a.StartEvent != dateTimeItem.StartEvent))
                    {
                        _context.EventScheduleDateTimes.Remove(dateTimeItem);
                    }
                }

                _context.Entry(scheduleDb).CurrentValues.SetValues(schedule);
                _context.SaveChanges();
            }
            else
            {
                var tempScheduleDb = _context.EventSchedules.Add(schedule);
                _context.SaveChanges();
                eventScheduleId = tempScheduleDb.EventScheduleId;
            }

            foreach (var startEvent in schedule.StartEventList)
            {
                startEvent.EventScheduleId = eventScheduleId;
                if (
                    !_context.EventScheduleDateTimes.Any(
                        a =>
                            a.EventSchedule.EventScheduleId == startEvent.EventScheduleId &&
                            a.StartEvent == startEvent.StartEvent))
                {
                    _context.EventScheduleDateTimes.Add(startEvent);
                }
            }
            _context.SaveChanges();

            return true;
        }

        public bool EnableTimePeriod(int eventid)
        {
            return _context.Events.Any(w => w.EventId == eventid && w.TypeId == 1);
        }

        public List<Event> GetEventsBySearchText(string searchStr)
        {
            return _context.Events.Where(w => w.Title.Contains(searchStr)).ToList();
        }
    }
}
