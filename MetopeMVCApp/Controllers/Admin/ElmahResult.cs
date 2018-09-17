using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// this is a way of implement elmah in MVC using an MVC structure. also see the RouteConfig
//see http://beletsky.net/2011/03/integrating-elmah-to-aspnet-mvc-in.html
namespace MetopeMVCApp.Controllers.Admin
{
    class ElmahResult : ActionResult
    {
        private string _resouceType;

        public ElmahResult(string resouceType)
        {
            _resouceType = resouceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var factory = new Elmah.ErrorLogPageFactory();

            if (!string.IsNullOrEmpty(_resouceType))
            {
                var pathInfo = "/" + _resouceType;
                context.HttpContext.RewritePath(FilePath(context), pathInfo, context.HttpContext.Request.QueryString.ToString());
            }

            var currentApplication = (HttpApplication)context.HttpContext.GetService(typeof(HttpApplication));
            var currentContext = currentApplication.Context;

            var httpHandler = factory.GetHandler(currentContext, null, null, null);
            if (httpHandler is IHttpAsyncHandler)
            {
                var asyncHttpHandler = (IHttpAsyncHandler)httpHandler;
                asyncHttpHandler.BeginProcessRequest(currentContext, (r) => { }, null);
            }
            else
            {
                httpHandler.ProcessRequest(currentContext);
            }
        }

        private string FilePath(ControllerContext context)
        {
            return _resouceType != "stylesheet" ? context.HttpContext.Request.Path.Replace(String.Format("/{0}", _resouceType), string.Empty) : context.HttpContext.Request.Path;
        }
    }

    [Authorize(Roles = "Admin")] 
    public class ElmahController : Controller
    {
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }
    }
}