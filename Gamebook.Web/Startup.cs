using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gamebook.Web.Startup))]
namespace Gamebook.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
