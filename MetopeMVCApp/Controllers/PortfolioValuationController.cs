using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Data;

namespace MetopeMVCApp.Controllers
{
    public class PortfolioValuationController : Controller
    { 
        private readonly IPortfolioValuationRepository db11;  

        private MetopeDbEntities db = new MetopeDbEntities(); //REMOVE this when done doing repository

        public PortfolioValuationController(IPortfolioValuationRepository iDb)
        {
            db11 = iDb; 
        }
         

        // GET: PortfolioValuation
        public ActionResult Index()
        {
            var portfolio_Valuation = db.Portfolio_Valuation.Include(p => p.Entity).Include(p => p.Portfolio).Include(p => p.User);
            return View(portfolio_Valuation.ToList());
        }

        // GET: PortfolioValuation/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(id);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Create
        public ActionResult Create()
        {
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name");
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");
            return View();
        }

        // POST: PortfolioValuation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Portfolio_Code,Entity_ID,Net_Asset_Value,Gross_Asset_Value,Total_Investments,Total_Cash,Liabilities,Fees,Total_Cost,NAV_Excl_Fees,Last_Update_User,Last_Update_Date")] Portfolio_Valuation portfolio_Valuation)
        {
            if (ModelState.IsValid)
            {
                db.Portfolio_Valuation.Add(portfolio_Valuation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(id);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // POST: PortfolioValuation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Portfolio_Code,Entity_ID,Net_Asset_Value,Gross_Asset_Value,Total_Investments,Total_Cash,Liabilities,Fees,Total_Cost,NAV_Excl_Fees,Last_Update_User,Last_Update_Date")] Portfolio_Valuation portfolio_Valuation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portfolio_Valuation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", portfolio_Valuation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio_Valuation.Entity_ID);
            return View(portfolio_Valuation);
        }

        // GET: PortfolioValuation/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(id);
            if (portfolio_Valuation == null)
            {
                return HttpNotFound();
            }
            return View(portfolio_Valuation);
        }

        // POST: PortfolioValuation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Portfolio_Valuation portfolio_Valuation = db.Portfolio_Valuation.Find(id);
            db.Portfolio_Valuation.Remove(portfolio_Valuation);
            db.SaveChanges();
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
