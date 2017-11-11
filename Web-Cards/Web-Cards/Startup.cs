using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Cards.Startup))]
namespace Web_Cards
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
