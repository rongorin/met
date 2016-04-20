using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace ASP.MetopeNspace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
       [AllowAnonymous]  
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}