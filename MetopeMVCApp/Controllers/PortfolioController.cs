using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers
{
    public class PortfolioController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: /Portfolio/
        public ActionResult Index()
        {
            var portfolios = db.Portfolios.Include(p => p.Entity).Include(p => p.User);
            return View(portfolios.ToList());
        }

        // GET: /Portfolio/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // GET: /Portfolio/Create
        public ActionResult Create()
        {
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");
            return View();
        }

        // POST: /Portfolio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End,Custodian_Code,Active_Flag,System_Locked")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                db.Portfolios.Add(portfolio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio.Entity_ID);
            return View(portfolio);
        }

        // GET: /Portfolio/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio.Entity_ID);
            return View(portfolio);
        }

        // POST: /Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End,Custodian_Code,Active_Flag,System_Locked")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portfolio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", portfolio.Entity_ID);
            return View(portfolio);
        }

        // GET: /Portfolio/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // POST: /Portfolio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Portfolio portfolio = db.Portfolios.Find(id);
            db.Portfolios.Remove(portfolio);
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
