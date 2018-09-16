using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Metope.WebAPI.Startup))]
namespace Metope.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
