using System.Web.Mvc;
using System.Web.Routing;

namespace Moemisto.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "search/{query}", url: "search", defaults: new { controller = "Search", action = "Index", query = UrlParameter.Optional });
            routes.MapMvcAttributeRoutes();
            //routes.MapRoute(name: "event", url: "event", defaults: new { controller = "Event", action = "Index" });
            //routes.MapRoute(name: "news", url: "news", defaults: new { controller = "News", action = "Index" });
            routes.MapRoute(name: "newsDetails", url: "{url}", defaults: new { controller = "News", action = "Details" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
