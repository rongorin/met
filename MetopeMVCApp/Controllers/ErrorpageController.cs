using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Controllers
{
    public class ErrorpageController : Controller
    {
        public ActionResult Error(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            var error = new Models.Custom.MyError
            {
                StatusCode = statusCode.ToString(),
                StatusDescription = HttpWorkerRequest.GetStatusDescription(statusCode),
                Message = exception.ToString(),
                DateTime = DateTime.Now
            };
            return View(error);
        }

        // GET: /Errorpage/
        //public ActionResult Errormessage()
        //{
        //    return View();
        //}
	}
}