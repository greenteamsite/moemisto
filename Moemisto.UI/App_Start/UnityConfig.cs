using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Moemisto.Data.Contexts;
using Moemisto.Data.Contexts.Admin;
using Moemisto.UI.Controllers;
using Unity.Mvc5;

namespace Moemisto.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<HomeContext>();
            container.RegisterType<BaseContext>();
            container.RegisterType<AdminEventContext>();
            container.RegisterType<AdminNewsContext>();
            container.RegisterType<EventContext>();
            container.RegisterType<NewsContext>();
            container.RegisterType<PlaceContext>();
            container.RegisterType<SearchContext>();
            container.RegisterType<TravelContext>();
            container.RegisterType<FeedContext>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}