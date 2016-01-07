using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;

namespace Moemisto.UI.Controllers
{
    public class EventController : Controller
    {
        private readonly EventContext _context;
        public EventController(EventContext context)
        {
            _context = context;
        }
        [Route("~/afisha/type/{url}/{eventDate}")]
        [Route("~/afisha/type/{url}")]
        [Route("~/afisha")]
        [OutputCache(Duration = 1800)]
        public ActionResult Index(string url, string eventDate)
        {
            ViewBag.Keywords = "подорожі, київські прогулянки, туризм";
            ViewBag.Description = "Найцікавіші події в Києві: концерти, вистави, кіно, виставки і фестивалі. Ми знаємо, куди піти в Києві на вихідних!";
            var model = new EventIndexVm { Url = url, StartDate = string.IsNullOrEmpty(eventDate) ? DateTime.Now.ToString("dd_MM_yyyy") : eventDate };

            return View(model);
        }

        public ActionResult TypeBox(string eventDate, string eventTypeUrl)
        {
            ViewBag.EventDate = eventDate;
            var format = new CultureInfo("uk-UA");
            DateTime publish = eventDate == null ? DateTime.Now : DateTime.Parse(eventDate.Replace("_","."), format);

            var model =
                _context.GetEventTypesExist(publish)
                    .Select(s => new SelectListItem { Text = s.Title, Value = s.TranslitUrl, Selected = s.TranslitUrl == eventTypeUrl })
                    .ToList();
            model.Insert(0, new SelectListItem { Text = "Всі події", Value = "all", Selected = string.IsNullOrEmpty(eventTypeUrl) });

            return PartialView("_TypeBox", model);

        }

        [Route("~/afisha/cat/{id}")]
        public ActionResult CategoriesOld(int id)
        {
            string url = _context.GetEventTypeUrlById(id);
            return RedirectPermanent(string.Format("/afisha/type/{0}", url));
        }

        public ActionResult EventsBox(string eventDate, string eventTypeUrl)
        {
            var format = new CultureInfo("uk-UA");
            DateTime publish = eventDate == null ? DateTime.MinValue : DateTime.Parse(eventDate.Replace("_", "."), format);

            //var today = DateTime.Now;

            //if (publish == new DateTime(today.Year, today.Month, today.Day))
            //{
            //    publish = DateTime.MinValue;
            //}
            var model = Mapper.Map<List<EventBaseVm>>(_context.GetEventsByFiltersStoredPr(publish, eventTypeUrl));

            var places = _context.GetPlaceInfo(model.Select(s => s.EventId).ToList());

            Parallel.ForEach(model, eventItem =>
            {
                if (places.ContainsKey(eventItem.EventId))
                {
                    eventItem.PlaceUrl = places[eventItem.EventId].Item1;
                    eventItem.PlaceName = places[eventItem.EventId].Item2;
                }
            });
            
            return PartialView("_EventsBox", model);
        }

        [Route("~/afisha/places")]
        public ActionResult PlacesOld()
        {
            return RedirectToActionPermanent("Index","Place");
        }


        [Route("~/afisha/{url}")]
        [OutputCache(Duration = 3600)] 
        public ActionResult Details(string url)
        {
            var eventDb = Mapper.Map<EventBaseVm>(_context.GetEvent(url));
            if (eventDb == null)
            {
                return RedirectToAction("Index");
            }
            var schedules = _context.GetEventPlacesByEventId(eventDb.EventId).Select(s => new EventScheduleItemVm { PlaceId = s.PlaceId, PlaceTitle = s.Title, PlaceUrl = s.TranslitUrl, PlaceAddress = s.Address }).ToList();
            foreach (var scheduleItem in schedules)
            {
                scheduleItem.StartEventList = _context.GetEventSchedulesDateTimes(eventDb.EventId, scheduleItem.PlaceId);
            }

            var otherEvents = Mapper.Map<List<EventBaseVm>>(_context.GetOtherEvent(eventDb.EventId));

            var places = _context.GetPlaceInfo(otherEvents.Select(s => s.EventId).ToList());

            Parallel.ForEach(otherEvents, eventItem =>
            {
                if (places.ContainsKey(eventItem.EventId))
                {
                    eventItem.PlaceUrl = places[eventItem.EventId].Item1;
                    eventItem.PlaceName = places[eventItem.EventId].Item2;
                }

            });
            var model = new EventDetailsVm
            {
                Event = eventDb,
                OtherEvents = otherEvents,
                Schedules = schedules,
                StartDate = _context.GetEventStartDate(eventDb.EventId)
            };
            model.Event.Title = string.Format("{0} \"{1}\"", model.Event.CategoryName, model.Event.Title);

            ViewBag.Keywords = string.Format("подія, {0}", model.Event.Title.Split(' ').FirstOrDefault());
            ViewBag.Description = model.Event.ShortInfoCutted(150);
            if (Request.Url != null)
            {
                ViewBag.OgImage = string.Format("{0}{1}",
                    Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, string.Empty), model.Event.PosterSmallUrl);
            }
            
            return View(model);
        }

    }
}