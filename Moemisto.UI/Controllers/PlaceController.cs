using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;

namespace Moemisto.UI.Controllers
{
   public class PlaceController : Controller
    {
        private readonly PlaceContext _context;

       public PlaceController(PlaceContext context)
       {
           _context = context;
       }

       [Route("~/hot_tels")]
       public ActionResult IndexOld()
       {
           return RedirectToActionPermanent("Index");
       }

       [Route("~/places")]
       [OutputCache(Duration = 600)]
       public ActionResult Index()
        {
            ViewBag.Keywords = "подорожі, київські прогулянки, туризм";
            ViewBag.Description = "Всі місця Києва: гарячі телефони та контакти державних установ та закладів відпочинку. Найповніша база установ та організацій Києва.";
            var model = new PlaceIndexVm
            {
                Groups =  Mapper.Map<List<PlaceTypeBaseVm>>(_context.GetGroups())
            };
            return View(model);
        }
       [Route("~/places/{groupUrl}")]
       [OutputCache(Duration = 600)]
       public ActionResult Types(string groupUrl)
        {
            var model = new PlaceTypesVm
            {
                Types = Mapper.Map<List<PlaceTypeBaseVm>>(_context.GetTypes(groupUrl)),
                GroupTitle = _context.GetGroupTitle(groupUrl)
            };
           foreach (var typeItem in model.Types)
           {
               typeItem.GroupUrl = groupUrl;
           }
           ViewBag.Keywords = string.Format("місця, {0}", model.GroupTitle);
           ViewBag.Description = model.GroupTitle;
           return View(model);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="groupUrl"></param>
       /// <param name="typeUrl"></param>
       /// <returns></returns>
       [Route("~/places/{groupUrl}/{typeUrl}")]
       [OutputCache(Duration = 600)]
       public ActionResult List(string groupUrl, string typeUrl)
       {
           var typePlace = _context.GetType(typeUrl);
            var model = new PlaceListVm
            {
                PlaceList = Mapper.Map<List<PlaceBaseVm>>(_context.GetPlaces(typePlace.PlaceTypeId)),
                GroupTitle = _context.GetGroupTitleByTypeId(typePlace.PlaceTypeId),
                TypeTitle = typePlace.Title,
                TypeTitleOne = typePlace.TitleOne,
                PlaceTypeId = typePlace.PlaceTypeId,
                PlaceTypeGroupId = typePlace.PlaceTypeGroupId,
                GroupUrl = groupUrl
            };
            ViewBag.Keywords = string.Format("{0},{1}", model.TypeTitleOne, model.GroupTitle);
            ViewBag.Description = model.TypeTitleOne;

            return View(model);
        }

       [Route("~/place/{url}")]
       [OutputCache(Duration = 3600)]
       public ActionResult Details(string url)
        {
            var modelDb = _context.GetPlaceDetails(url);
            if (modelDb == null)
            {
                return RedirectToAction("Index");
            }
            var model = Mapper.Map<PlaceDetailsVm>(modelDb);
            model.GroupTitle = _context.GetGroupTitleByTypeId(model.PlaceTypeId);
            model.PlaceEvents = Mapper.Map<List<EventBaseVm>>(_context.GetPlaceEvent(model.PlaceId));
            model.OtherPlaces = Mapper.Map<List<PlaceBaseVm>>(_context.GetOtherPlaces(model.PlaceId, model.PlaceTypeId));
            model.GroupUrl = _context.GetGroupUrlByTypeId(model.PlaceTypeId);
            model.TypeUrl = _context.GetTypeUrlByTypeId(model.PlaceTypeId);
            ViewBag.Keywords = string.Format("{0},{1}", model.TypeTitleOne, model.GroupTitle);
            ViewBag.Description = model.ShortInfoCutted(150);
            if (Request.Url != null)
            {
                ViewBag.OgImage = string.Format("{0}{1}",
                    Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, string.Empty), model.PosterSmallUrl);
            }
            return View(model);
        }

        [Route("~/afisha/place/{id}")]
        [Route("~/hot_tels/{id}")]
        public ActionResult DetailsOld(int id)
        {
            string url = _context.GetPlaceUrlById(id);
            return RedirectToActionPermanent("Details", new { url = url });
        }

    }
}