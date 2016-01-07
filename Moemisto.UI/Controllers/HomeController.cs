using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Moemisto.Data.Contexts;
using Moemisto.UI.Models;
using Moemisto.UI.Services.Feed;
using Terradue.ServiceModel.Syndication;

namespace Moemisto.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeContext _context;
        private readonly FeedContext _contextFeed;
        public HomeController(HomeContext context, FeedContext contextFeed)
        {
            _context = context;
            _contextFeed = contextFeed;
        }

        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            ViewBag.Keywords = "новини, київські новини, статті, події, аналітика, афіша Києва, сервіси";
            ViewBag.Description = "Моє Місто - це цікаві новини про Київ і афіша цікавих подій у Києві. Інтернет-сайт новин про головні київські новини і те, куди піти в Києві на вихідних. Читати столичні новини варто тут!";
            var model = new HomeIndexVm
            {
                TopNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetTopNews()),
                LastNews = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastNews()),
                InterestedEvents = Mapper.Map<List<EventBaseVm>>(_context.GetInterestedEvents()),
                LastTravels = Mapper.Map<List<ArticleBaseVm>>(_context.GetTopTravels()),
            };

            var places = _context.GetPlaceInfo(model.InterestedEvents.Select(s => s.EventId).ToList());

            Parallel.ForEach(model.InterestedEvents, eventItem =>
            {
                if (places.ContainsKey(eventItem.EventId))
                {
                    eventItem.PlaceUrl = places[eventItem.EventId].Item1;
                    eventItem.PlaceName = places[eventItem.EventId].Item2;
                }
            });

            return View(model);
        }

        [Route("~/map")]
        [Route("~/mplace")]
        [Route("~/recommend")]
        public ActionResult RecommendOld()
        {
            return RedirectToActionPermanent("Index");
        }

        [Route("~/adv")]
        public ActionResult AdvOld()
        {
            return RedirectToActionPermanent("About");
        }

        [Route("~/about")]
        public ActionResult About()
        {
            return View();
        }

        [OutputCache(Duration = 3600)]
        public ActionResult BottomMenu()
        {
            var model = new HomeIndexBottomMenuVm
            {
                News = _context.GetBottomMenuNews(),
                Events = _context.GetBottomMenuEvents(),
                Travels = _context.GetBottomMenuTravels(),
                Places = _context.GetBottomMenuPlaces(),
            };
            return PartialView("_BottomMenu", model);
        }

        [OutputCache(Duration = 3600)]
        public ActionResult TopMenu()
        {
            var model = new HomeIndexBottomMenuVm
            {
                News = _context.GetBottomMenuNews(),
                Events = _context.GetBottomMenuEvents(),
                Travels = _context.GetBottomMenuTravels(),
                Places = _context.GetBottomMenuPlaces(),
            };
            return PartialView("_TopMenu", model);
        }

        [OutputCache(Duration = 3600)]
        [Route("~/rss")]
        public async Task<ActionResult> Feed()
        {
            var feedService = new FeedService(_contextFeed);
            CancellationToken cancellationToken = Response.ClientDisconnectedToken;
            SyndicationFeed feed = await feedService.GetFeed(cancellationToken);
            return new RssActionResult(feed);
        }

        public ActionResult GetMoreNews(int id)
        {
            var model = Mapper.Map<List<ArticleBaseVm>>(_context.GetLastNewsAfterId(id));
            return PartialView("MoreNewsBox", model);
        }
    }
}