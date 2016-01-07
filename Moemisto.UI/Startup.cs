using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Moemisto.UI.Startup))]
namespace Moemisto.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
