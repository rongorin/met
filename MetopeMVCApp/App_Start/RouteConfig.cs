using MetopeMVCApp.App_Start;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASP.MetopeNspace
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {  
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add("UserRoute", new UserRoute());
             
            routes.MapRoute(
                "Admin_elmah",
                "Admin/elmah/{type}",
                new { controller = "Elmah", action = "Index", type = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
             

        }
    }
}