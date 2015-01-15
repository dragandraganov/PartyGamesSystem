using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PartyGamesSystem.Web.Startup))]
namespace PartyGamesSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
