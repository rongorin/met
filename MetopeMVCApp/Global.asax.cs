
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ASP.MetopeNspace
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(decimal?), new MetopeMVCApp.DecimalModelBinder());
            MiniProfilerEF6.Initialize();
        }
        protected void Application_BeginRequest()
        {
            var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = " ";
            currentCulture.NumberFormat.CurrencyDecimalSeparator = "."; 
            Thread.CurrentThread.CurrentCulture = currentCulture;

            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            } 

            //Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
        //protected  void Application_Error(object sender, EventArgs e) {
        //    Exception exc = Server.GetLastError(); 
        //    Server.ClearError();
        //    Response.Redirect("/Errorpage/Errormessage");
        //}
    }
}
