using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Metope.DAL;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class ForexForecastController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly IForexForecastRepository db11;

        public ForexForecastController(IForexForecastRepository iDb)
        {
            db11 = iDb;
        }
         
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            var vm = db11.GetAllRecs(x => x.Entity_ID == EntityID)
                    .OrderBy(X => X.Security_ID).ThenByDescending(x=> x.Month_Year);

            return View(vm);
        }

         
        // GET: ForexForecast/Create
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create( string Nav = "")
        {   
            decimal EntityID = (decimal)ViewBag.EntityId;
            var Ff= new Forex_Forecast
            {
                Entity_ID = EntityID
            };
            ViewBag.Nav = Nav;

            return View(Ff); 

        }

        // POST: ForexForecast/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_ID,Month_Year,Forecast_Rate,Last_Update_Date,Last_Update_User")] 
                                    Forex_Forecast forex_Forecast,  string navIndicator = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            forex_Forecast.Entity_ID = EntityID;
            /*------------------------------------------ 
            first check if this party code is already used ! 
             ----------------------------------------*/
            Forex_Forecast check = db11.FindBy(r => r.Security_ID == forex_Forecast.Security_ID &&
                                                    r.Entity_ID == forex_Forecast.Entity_ID &&
                                                    r.Month_Year == forex_Forecast.Month_Year).FirstOrDefault<Forex_Forecast>();             
            if (ModelState.IsValid)
            { 
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Forex forecast for security " + forex_Forecast.Security_ID.ToString() + " as it Already exists!");
                }
                else
                { 
                     forex_Forecast.Last_Update_Date = DateTime.Now;
                     forex_Forecast.Last_Update_User = User.Identity.Name; 
                     
                     db11.Add(forex_Forecast);
                     db11.Save();
                     TempData["ResultMessage"] = "Forex forecast for Security " + forex_Forecast.Security_ID.ToString() + " created successfully!";
  

                     if (navIndicator == "")
                         return RedirectToAction("Index", null, new { SecurityId = forex_Forecast.Security_ID });
                     else 
                         return RedirectToAction("Index", null, new { SecurityId = forex_Forecast.Security_ID });
                }
            }  
            return View(forex_Forecast);
        }

      

        // GET: ForexForecast/Edit/5
        [CustomEntityAuthoriseFilter]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(decimal EntityId, decimal SecurityId, string MonthYear)
        {
            Forex_Forecast ff = db11.FindBy(r => r.Entity_ID == EntityId && r.Security_ID == SecurityId &&
                                r.Month_Year == MonthYear).FirstOrDefault();

            if (ff == null)
            {
                return HttpNotFound();
            }
             
            ViewBag.SecuritiesAll = ff.Security_ID;

            //if (Request.IsAjaxRequest())
            //{
            //    return View(party);
            //}
            ViewBag.SecuritiesAll = ff.Security_ID;
            return View(ff); 
        } 


        // POST: ForexForecast/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_ID,Month_Year,Forecast_Rate,Last_Update_Date,Last_Update_User")] Forex_Forecast forex_Forecast)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            if (ModelState.IsValid)
            {
                db11.Update(forex_Forecast);
                forex_Forecast.Last_Update_Date = DateTime.Now;
                forex_Forecast.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Forex Forecast for security  " + forex_Forecast.Security_ID.ToString() + " edited successfully!";

                return RedirectToAction("Index", null, new { SecurityId = forex_Forecast.Security_ID });
          
            }
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", forex_Forecast.Security_ID);
           // ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", forex_Forecast.Entity_ID);
            return View(forex_Forecast);
        }
      
        // GET: ForexForecast/Delete/5
        public ActionResult Delete( decimal SecurityId, string MonthYear) 
        { 
            var EntityID = (decimal)ViewBag.EntityId;
            Forex_Forecast  forex_Forecast = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                r.Entity_ID == EntityID &&
                                                r.Month_Year == MonthYear
                ).FirstOrDefault();

            if (forex_Forecast == null)
            {
                return HttpNotFound();
            }

            return View(forex_Forecast); 
        }

        // POST: ForexForecast/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public ActionResult DeleteConfirmed( decimal SecurityId, string MonthYear) 
        { 

            var EntityID = (decimal)ViewBag.EntityId;
            Forex_Forecast forex_Forecast = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                r.Entity_ID == EntityID &&
                                                r.Month_Year == MonthYear
                                            ).FirstOrDefault();

            db11.Delete(forex_Forecast);
            db11.Save();
            TempData["ResultMessage"] = "Forex Forecast for Security " + forex_Forecast.Security_ID.ToString() + " deleted successfully!";

            return RedirectToAction("Index", null,   new { SecurityId = forex_Forecast.Security_ID }); 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
