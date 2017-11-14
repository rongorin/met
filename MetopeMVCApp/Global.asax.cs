
using MetopeMVCApp.Controllers;
using MetopeMVCApp.Filters.DbInterceptors;
using MetopeMVCApp.Models.MyMetaData;
using NLog;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Globalization;
using System.Linq;
using System.Text;
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
            
            DbInterception.Add(new NLogCommandInterceptor()); //my custom Commmand

            ModelBinders.Binders.Add(typeof(decimal?), new MetopeMVCApp.DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new MetopeMVCApp.DecimalModelBinder());
            ModelMetadataProviders.Current = new MyMetadataProvider();
            Application["TestMode"] = ConfigurationManager.AppSettings["TestMode"]; 
            MiniProfilerEF6.Initialize();
        }
        protected void Application_BeginRequest()
        {
            var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = ",";
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
         

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exc = Server.GetLastError();
            //Server.ClearError();
            //Response.Redirect("/Errorpage/Errormessage");

            /*---------------------------------------------------------------------------------------
             * implement logging method from : http://blog.chudinov.net/errors-handling-and-logging-in-asp-net-mvc/
             --------------------------------------------------------------------------------------*/
            Exception ex = Server.GetLastError();
	        if (ex != null)
	        {

                Logger logger = LogManager.GetCurrentClassLogger(); 

                StringBuilder errPart2 = new StringBuilder();
                if (null != ex.InnerException)
                    errPart2.Append("\nInner Error Message:" + ex.InnerException.Message);

                errPart2.Append("\nStack Trace:" + ex.StackTrace);
                Server.ClearError();
                
                if (null != Context.Session)
                {
                     errPart2.Append("\nSession: Identity name:");
                     errPart2.Append(Thread.CurrentPrincipal.Identity.Name);
                     errPart2.Append(" IsAuthenticated:" );
                     errPart2.Append(Thread.CurrentPrincipal.Identity.IsAuthenticated);
                }

                logger.Error("Error caught in Application_Error event\n, {0} \n Error in: {1}, {2} ", (Context.Session == null ? string.Empty : Request.Url.ToString()),
                                                                                              ex.Message,
                                                                                              errPart2);
                //StringBuilder err = new StringBuilder();
                //err.Append("Error caught in Application_Error event\n");
                //err.Append("Error in: " + (Context.Session == null ? string.Empty : Request.Url.ToString()));
                //err.Append("\nError Message:" + ex.Message);
                //if (null != ex.InnerException)
                //    err.Append("\nInner Error Message:" + ex.InnerException.Message);
                //err.Append("\n\nStack Trace:" + ex.StackTrace);
                //Server.ClearError();

                //if (null != Context.Session)
                //{
                //     err.Append($"Session: Identity name:[{Thread.CurrentPrincipal.Identity.Name}] IsAuthenticated:{Thread.CurrentPrincipal.Identity.IsAuthenticated}");
                //}
                ////logger.Error(err.ToString());
                //logger.Error("Sample errr message");

		        if (null != Context.Session)
		        {
			        var routeData = new RouteData();
			        routeData.Values.Add("controller", "Errorpage");
			        routeData.Values.Add("action", "Error");
			        routeData.Values.Add("exception", ex);

			        if (ex.GetType() == typeof(HttpException))
			        {
				        routeData.Values.Add("statusCode", ((HttpException)ex).GetHttpCode());
			        }
			        else
			        {
				        routeData.Values.Add("statusCode", 500);
			        }
			        Response.TrySkipIisCustomErrors = true;
                    IController controller = new ErrorpageController();
			        controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
			        Response.End();
 
		        }
	        }
       
        }
    }
}
