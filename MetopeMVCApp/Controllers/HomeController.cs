using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Models;
using System.Net;
using MetopeMVCApp.Models.Custom;
using MetopeMVCApp.Filters;
using System.Configuration;
using Metope.DAL; 

namespace ASP.MetopeNspace.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
         
        public ActionResult Index()
        { 

            //var userId = User.Identity.GetUserId();
            //var currentUser = manager.FindById(userId);
             
            // MetopeDbEntities db = new MetopeDbEntities(); 
            //Entity entity = db.Entities.Find(currentUser.EntityIdScope);

            var strEntityName = GetEntityName(User.Identity.GetUserId());
            if (strEntityName == "")
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
              
            Session["EntityInnScope"] = strEntityName; 



            return View();
        }
        public ActionResult Support()
        {
            ViewBag.Message = "24 Hour Support";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "General stuff.";
           
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       private string GetEntityName(string iUuserId)
       { 
           var currentUser = manager.FindById(iUuserId); 
           MetopeDbEntities db = new MetopeDbEntities();
           Entity entity = db.Entities.Find(currentUser.EntityIdScope);
           if (entity == null)
               return "";
           else
              return entity.Entity_Name;      

       }
       public JsonResult GetWeather()  // --> this is called by javascript Ajax call.
       {
           Weather weath = new Weather();
          
           return Json(weath.getWeatherForecast(), JsonRequestBehavior.AllowGet);
       }

       public ActionResult EntityInScope()
       { 
           //return Content(MetopeMVCApp.Data.Constants.EntityNameInScope);

           if (Session["EntityInnScope"] == null)
           {
               var strEntityName = GetEntityName(User.Identity.GetUserId());
               if (strEntityName == "")
                   return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);

               Session["EntityInnScope"] = strEntityName;


           }
           if (Session["EntityInnScope"] == null) 
               Session["EntityInnScope"] = ConfigurationManager.AppSettings["TestMode"]; 

          return Content( Session["EntityInnScope"].ToString());

           // various return types: see the Response types that can be returned on microsoft website
           //return new HttpStatusCodeResult(403);
           //return Json(new { name = "serial",value = "737373" } , JsonRequestBehavior.AllowGet);
           //return ViewPartial();
       }
       public ActionResult ShowTestMode()
       {
            
           return Content("TESTING MODE".ToString()); 
            
       }
    }
}