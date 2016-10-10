using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Controllers
{
    public class ErrorpageController : Controller
    {
        //
        // GET: /Errorpage/
        public ActionResult Errormessage()
        {
            return View();
        }
	}
}