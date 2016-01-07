using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Moemisto.Data;
using Moemisto.Data.Migrations;

namespace Moemisto.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbMmContext, MmConfiguration>());

            AutoMapperConfig.RegisterMappings();
        }
    }
}
