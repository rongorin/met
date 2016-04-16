using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP.MetopeNspace.Startup))]
namespace ASP.MetopeNspace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
