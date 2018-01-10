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
    public class PortfolioValuationController : Controller
    { 
        private readonly IPortfolioValuationRepository db11;  

        private MetopeDbEntities db = new MetopeDbEntities(); //REMOVE this when done doing repository

        //public PortfolioValuationController(IPortfolioValuationRepository iDb)
        //{
        //    db11 = iDb; 
        //} 

        // GET: PortfolioValuation
        public ActionResult Index()
        {
            var portfolio_Valuation = db.Portfolio_Valuation.Include(p => p.Entity).Include(p => p.Portfolio)
                                                      .OrderBy(s => s.Portfolio_Code);
            return View(portfolio_Valuation.ToList());
        }

        // GET: PortfolioValuation/Details/5
        public ActionResult Details(decimal EntityId, string PortfolioCode)
        {
            if (PortfolioCode == null || EntityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(PortfolioCode, EntityId);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Create
        [PortfoliosFilter]
        public ActionResult Create()
        {
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Portfolio_Code", "Portfolio_Name");
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");
            return View();
        }

        // POST: PortfolioValuation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PortfoliosFilter]
        public ActionResult Create([Bind(Include = "Portfolio_Code,Entity_ID,Net_Asset_Value,Gross_Asset_Value,Total_Investments,Total_Cash,Liabilities,Fees,Total_Cost,NAV_Excl_Fees,Last_Update_User,Last_Update_Date")] Portfolio_Valuation portfolio_Valuation)
        {

            decimal EntityID = (decimal)ViewBag.EntityId;
            portfolio_Valuation.Entity_ID = EntityID; 
            if (ModelState.IsValid)
            {
                /*------------------------------------------ 
                first check if this party code is already used ! 
                ----------------------------------------*/
                Portfolio_Valuation check = db.Portfolio_Valuation.Find(portfolio_Valuation.Portfolio_Code, EntityID);
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Portfolio Valuation for \"" + portfolio_Valuation.Portfolio_Code + "\"  as it Already exists!");
                }
                else
                {
                    db.Portfolio_Valuation.Add(portfolio_Valuation);
                    db.SaveChanges();
                    TempData.Add("ResultMessage", "new Valuation for Porfolio " + portfolio_Valuation.Portfolio_Code + " created successfully!");
                    return RedirectToAction("Index");
                }
            }

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Code", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Edit/5
        [PortfoliosFilter]
        public ActionResult Edit(decimal EntityId, string PortfolioCode)
        {
            if (PortfolioCode == null || EntityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(PortfolioCode, EntityId);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }

            ViewBag.myPortfolioCode = portfolio_Valuation.Portfolio_Code; 
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // POST: PortfolioValuation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PortfoliosFilter]
        public ActionResult Edit([Bind(Include = "Portfolio_Code,Entity_ID,Net_Asset_Value,Gross_Asset_Value,Total_Investments,Total_Cash,Liabilities,Fees,Total_Cost,NAV_Excl_Fees,Last_Update_User,Last_Update_Date")] Portfolio_Valuation portfolio_Valuation)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            portfolio_Valuation.Entity_ID = EntityID;

            if (ModelState.IsValid)
            {
                db.Entry(portfolio_Valuation).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ResultMessage"] = "Valuation for Portfolio " + portfolio_Valuation.Portfolio_Code.ToString() + " edited successfully!";
                return RedirectToAction("Index");
            }
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", portfolio_Valuation.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Delete/5
        public ActionResult Delete(decimal EntityId, string PortfolioCode)
        {
            if (PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(PortfolioCode, EntityId);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }
            return View(portfolio_Valuation);
        }

        // POST: PortfolioValuation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal EntityId, string PortfolioCode)
        {
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(PortfolioCode, EntityId);
            db.Portfolio_Valuation.Remove(portfolio_Valuation);
            db.SaveChanges();
            TempData["ResultMessage"] = "Valuation for Portfolio " + PortfolioCode + " deleted successfully!";
            return RedirectToAction("Index");
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
