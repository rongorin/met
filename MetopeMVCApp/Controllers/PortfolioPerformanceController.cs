using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Data;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using MetopeMVCApp.Filters;
using Microsoft.AspNet.Identity.EntityFramework;

using Metope.DAL;
namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PortfolioPerformanceController : Controller
    {
        private readonly IPortfolioPerformanceRepository db11; 
         
        public PortfolioPerformanceController(IPortfolioPerformanceRepository iDb)
        {
            db11 = iDb;
        }
        // GET: PortfolioPerformance
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
             
            var vm = db11.GetAllRecs(x => x.Entity_ID == EntityID); 

            return View(vm );
        }

        // GET: PortfolioPerformance/Create
         [PortfoliosFilter]
        public ActionResult Create(string PortfolioCode ) 
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
            ViewBag.myPortfolioCode = PortfolioCode;
              
            ViewBag.EntityIdScope = ViewBag.EntityId; 
              
            return View();
        }

        // POST: PortfolioPerformance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PortfoliosFilter]
         public ActionResult Create([Bind(Include = "Entity_ID,Portfolio_Code,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Dietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,ModDietz_Performance_Quarterly,Last_Update_Date,Last_Update_User")] Portfolio_Performance portfolio_Performance)
         {
            decimal EntityID = (decimal)ViewBag.EntityId;
            portfolio_Performance.Entity_ID = EntityID;
            /*------------------------------------------ 
             first check if this party code is already used ! 
             ------------------------------------------*/  
            Portfolio_Performance check = db11.FindBy(r => r.Portfolio_Code == portfolio_Performance.Portfolio_Code &&
                                                        r.Entity_ID == portfolio_Performance.Entity_ID).FirstOrDefault<Portfolio_Performance>();
            if (ModelState.IsValid)
            {
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Performance record for Portfolio " + portfolio_Performance.Portfolio_Code + " as it Already exists!");
                }
                else
                {
                    portfolio_Performance.Last_Update_Date = DateTime.Now;
                    portfolio_Performance.Last_Update_User = User.Identity.Name;

                    db11.Add(portfolio_Performance);
                    db11.Save();
                    TempData["ResultMessage"] = "Performance for Portfolio " + portfolio_Performance.Portfolio_Code + " created successfully!";

                    return RedirectToAction("Index");
                    //return RedirectToAction("Index", "Portfolio");
                }
            }

            return View(portfolio_Performance);
        }

        // GET: PortfolioPerformance/Edit/5
        [CustomEntityAuthoriseFilter]
        [PortfoliosFilter]
        public ActionResult Edit(decimal EntityId, string PortfolioCode, string Nav = "")
        {
            //var EntityID = (decimal)ViewBag.EntityId;

            var Portfolio_Performance = db11.FindBy(r => r.Portfolio_Code == PortfolioCode)
                                           .MatchCriteria(c => c.Entity_ID == EntityId).FirstOrDefault();

            if (Portfolio_Performance == null)
            {
                return HttpNotFound();
            }

            ViewBag.Nav = Nav;

            return View(Portfolio_Performance);
        }

        // POST: PortfolioPerformance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PortfoliosFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Portfolio_Code,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Dietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,ModDietz_Performance_Quarterly,Last_Update_Date,Last_Update_User")] Portfolio_Performance portfolio_Performance)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(portfolio_Performance); //sets the modified status 
                portfolio_Performance.Last_Update_Date = DateTime.Now;
                portfolio_Performance.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Performance for Portfolio " + portfolio_Performance.Portfolio_Code + " edited successfully!";
                 
                    return RedirectToAction("Index");

            }
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", Portfolio_Performance.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", Portfolio_Performance.Security_ID);
            //ViewBag.Discount_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", Portfolio_Performance.Discount_Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", Portfolio_Performance.Entity_ID);
            return View(portfolio_Performance);
        }

        // GET: PortfolioPerformance/Delete/5
        public ActionResult Delete(string PortfolioCode)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Portfolio_Performance saa = db11.FindBy(r => r.Portfolio_Code == PortfolioCode && r.Entity_ID == EntityID
                   ).FirstOrDefault();

            if (saa == null)
            {
                return HttpNotFound();
            }
            return View(saa);
        }

        // POST: PortfolioPerformance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(string PortfolioCode)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Portfolio_Performance Portfolio_Performance = db11.FindBy(r => r.Portfolio_Code == PortfolioCode && r.Entity_ID == EntityID
                   ).FirstOrDefault();

            db11.Delete(Portfolio_Performance);
            db11.Save();
            TempData["ResultMessage"] = "Performance for Portfolio" + Portfolio_Performance.Portfolio_Code + " deleted successfully!";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db11.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
