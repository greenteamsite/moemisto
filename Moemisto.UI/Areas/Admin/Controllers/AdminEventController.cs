using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts.Admin;
using Moemisto.Data.Entities;
using Moemisto.UI.Areas.Admin.Models;
using Moemisto.UI.Helpers;
using Moemisto.UI.Models;

namespace Moemisto.UI.Areas.Admin.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class AdminEventController : Controller
    {
        private readonly AdminEventContext _context;

        public AdminEventController(AdminEventContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var model = new AdminEventIndexVm
            {
                EventTypes = _context.GetEventTypes().Select(s => new SelectListItem { Text = s.Title, Value = s.EventTypeId.ToString() }).ToList(),
                EventPlaces = _context.GetEventPlaces().Select(s => new SelectListItem { Text = s.Title, Value = s.PlaceId.ToString() }).ToList(),
            };
            model.EventTypes.Insert(0, new SelectListItem { Text = "Всі події", Value = "0", Selected = true });
            model.EventPlaces.Insert(0, new SelectListItem { Text = "Всі місця", Value = "0", Selected = true });
            return View(model);
        }

        public ActionResult EventsBox(string eventDate, int eventTypeId = 0, string searchStr = "")
        {
            List<EventBaseVm> model;
            if (searchStr.Length > 2)
            {
                model = Mapper.Map<List<EventBaseVm>>(_context.GetEventsBySearchText(searchStr));
            }
            else
            {
                var format = new CultureInfo("uk-UA");
                DateTime publish = String.IsNullOrEmpty(eventDate) ? DateTime.MinValue : DateTime.Parse(eventDate, format);

                model = Mapper.Map<List<EventBaseVm>>(_context.GetEventsByFilters(publish, eventTypeId));
            }

            return PartialView("_EventsBox", model);
        }

        public ActionResult CreateEvent()
        {
            int eventId = _context.CreateEmptyEvent();
            return RedirectToAction("EventEdit", new { eventId = eventId });
        }

        [HttpGet]
        public ActionResult EventEdit(int eventId)
        {
            ViewBag.Title = "Редагування / Створення";
            var model = new AdminEventEditVm
            {
                Event = Mapper.Map<EventBaseVm>(_context.GetEvent(eventId)),
                Schedules = Mapper.Map<List<EventScheduleVm>>(_context.GetEventSchedules(eventId)),
                EventTypes = _context.GetEventTypesAll()
                    .Select(s => new SelectListItem {Text = s.Title, Value = s.EventTypeId.ToString()})
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult EventEdit([Bind(Prefix = "Event")] EventBaseVm model)
        {
            if (ModelState.IsValid)
            {
                var eventForDb = Mapper.Map<Event>(model);
                _context.SaveEvent(eventForDb);
                return Content("true");
            }
            return Content("false");
        }

        public ActionResult EventRemove(int eventId)
        {
            _context.EventRemove(eventId);
            return Content(String.Empty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures(int eventId, HttpPostedFileBase pictureTop)
        {
            DateTime today = DateTime.Now;
            string articlePath = String.Format("/Images/{0}/{1}/{2}/{3}/", today.Year, today.Month, today.Day, eventId);
            var path = Server.MapPath(articlePath);
            if (pictureTop != null && pictureTop.ContentLength > 0)
            {
                // extract only the fielname
                string fileName = Path.GetFileName(pictureTop.FileName);
                if (!String.IsNullOrEmpty(fileName))
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    pictureTop.SaveAs(Path.Combine(Server.MapPath(articlePath), fileName));
                    // Resize picture
                    using (var smallPicture = PictureResizer.ResizeImage(pictureTop.InputStream, 260))
                    {
                        smallPicture.Save(Path.Combine(Server.MapPath(articlePath), String.Format("s_{0}", fileName)),
                            ImageFormat.Jpeg);
                    }

                    var model = Mapper.Map<EventBaseVm>(_context.AddPicture(eventId, articlePath, fileName, String.Format("s_{0}", fileName)));
                    return PartialView("_PicturePreView", model);
                }
            }
            
            return Content(String.Empty);
        }
        public ActionResult RemovePicture(int eventId)
        {
            _context.RemovePicture(eventId);
            return Content(String.Empty);
        }

        public ActionResult EventSchedule(int eventid)
        {
            var model = new AdminEventScheduleVm
            {
                EventId = eventid,
                EventTitle = _context.GetEventTitle(eventid),
            };
            return View(model);
        }

        public ActionResult PlaceTypeBox(int typegroupid)
        {
            var model = _context.GetPlaceTypesByGroup(typegroupid).Select(s => new SelectListItem { Text = s.Title, Value = s.PlaceTypeId.ToString() }).ToList();
            return PartialView("_PlaceTypesBox", model);
        }
        public ActionResult PlacesByType(int typeid, int typeGroupId, int scheduleid)
        {
            if (typeid == 0)
            {
                typeid = _context.GetPlaceTypeIdFirstByGroup(typeGroupId);
            }
            int placeIdSelected = scheduleid == 0 ? 0 : _context.GetPlaceIdByScheduleId(scheduleid);
            var model =
                _context.GetEventPlacesByType(typeid)
                    .Select(s => new SelectListItem { Text = s.Title, Value = s.PlaceId.ToString(), Selected = s.PlaceId == placeIdSelected })
                    .ToList();

            if (placeIdSelected == 0)
            {
                model.First().Selected = true;
            }
            return PartialView("_PlaceListBox", model);
        }

        [HttpGet]
        public ActionResult EventScheduleItem(int scheduleId, int eventId)
        {
            var plaseTypeGroups = _context.GetPlaceTypeGroups().Select(s => new SelectListItem { Text = s.Title, Value = s.PlaceTypeGroupId.ToString() })
                    .ToList();

            int scheduleIdSetup = scheduleId;
            if (scheduleId == 0)
            {
                scheduleIdSetup = _context.GetScheduleIdByEventId(eventId);
            }
            var plaseTypeGroupFirst = scheduleIdSetup == 0 ? plaseTypeGroups.FirstOrDefault() : plaseTypeGroups.SingleOrDefault(w => w.Value == _context.GetPlaceTypeGroupIdByScheduleId(scheduleIdSetup).ToString());
            var placeTypeFirst = _context.GetPlaceTypeByScheduleId(scheduleIdSetup);

            var model = new AdminEventScheduleItemVm
            {
                EventId = eventId,
                ScheduleId = scheduleId,
                PlaceTypeGroups = plaseTypeGroups,
                PlaceTypeGroupFirst = plaseTypeGroupFirst,
                PlaceTypeFirst = new SelectListItem { Text = placeTypeFirst.Title, Value = placeTypeFirst.PlaceTypeId.ToString() }
            };
            return PartialView("_EventScheduleItem", model);
        }

        /// <summary>
        /// Save Schedule
        /// </summary>
        /// <param name="model">Schedule</param>
        /// <returns>Redirect To Action EventScheduleItem</returns>
        [HttpPost]
        public ActionResult EventScheduleItem(AdminEventScheduleBaseVm model)
        {
            TempData["AddDatePeriodFromInput"] = model.AddDatePeriodFromInput;
            TempData["AddDatePeriodToInput"] = model.AddDatePeriodToInput;
            TempData["AddTimesPeriodInput"] = model.AddTimesPeriodInput;

            if (model.StartEventList == null)
            {
                model.StartEventList = new List<DateTime>();
            }
            _context.SaveSchedule(Mapper.Map<EventSchedule>(model));

            return RedirectToAction("EventScheduleItem", new { scheduleId = model.EventScheduleId, eventId = 0 });
        }

        public ActionResult ScheduleList(int eventid)
        {
            var model = Mapper.Map<List<AdminEventScheduleBaseVm>>(_context.GetEventSchedules(eventid));
            return PartialView("_ScheduleList", model);
        }


        public ActionResult ScheduleEdit(int scheduleid, int eventid)
        {
            var model = scheduleid == 0
                ? new AdminEventScheduleBaseVm {StartEventList = new List<DateTime>()}
                : Mapper.Map<AdminEventScheduleBaseVm>(_context.GetEventScheduleItem(scheduleid));

            if (model.EventId == 0)
            {
                model.EventId = eventid;
            }

            if (TempData.ContainsKey("AddDatePeriodFromInput"))
            {
                var t = TempData.Peek("AddDatePeriodFromInput");
                model.AddDatePeriodFromInput = t == null ? "" : t.ToString();
            }
            if (TempData.ContainsKey("AddDatePeriodToInput"))
            {
                var t = TempData.Peek("AddDatePeriodToInput");
                model.AddDatePeriodToInput = t == null ? "" : t.ToString();
            }
            if (TempData.ContainsKey("AddTimesPeriodInput"))
            {
                var t = TempData.Peek("AddTimesPeriodInput");
                model.AddTimesPeriodInput = t == null ? "" : t.ToString();
            }

            model.EnableTimePeriod = _context.EnableTimePeriod(eventid);

            return PartialView("_ScheduleEdit", model);
        }


        public ActionResult ScheduleRemove(int scheduleid, int eventid)
        {
            _context.ScheduleRemove(scheduleid);
            return RedirectToAction("ScheduleList", new { eventid });
        }
    }
}